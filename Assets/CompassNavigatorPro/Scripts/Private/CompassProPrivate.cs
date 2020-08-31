using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System.Collections;
using System.Collections.Generic;

#if UNITY_5_4_OR_NEWER
using UnityEngine.SceneManagement;
#endif
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace CompassNavigatorPro {
				public delegate void OnPOIVisited (CompassProPOI poi);

				public delegate void OnPOIVisible (CompassProPOI poi);

				public delegate void OnPOIHide (CompassProPOI poi);

				[ExecuteInEditMode]
				public partial class CompassPro : MonoBehaviour {
								class CompassActiveIcon {
												public CompassProPOI poi;
												public Image image;
												public float lastPosX;
												public string levelName;
												RectTransform _rectTransform;

												public RectTransform rectTransform {
																get { return _rectTransform; }
																set {
																				_rectTransform = value;
																				image = _rectTransform.GetComponent<Image> ();
																}
												}

												public CompassActiveIcon (CompassProPOI poi) {
																this.poi = poi;
																#if UNITY_5_4_OR_NEWER
																this.levelName = SceneManager.GetActiveScene ().name;
																#else
				this.levelName = Application.loadedLevelName;
																#endif
												}
								}

								const float COMPASS_POI_POSITION_THRESHOLD = 0.001f;

								enum CardinalPoint {
												North = 0,
												West = 1,
												South = 2,
												East = 3
								}

								enum OrdinalPoint {
												NorthWest = 0,
												NorthEast = 1,
												SouthWest = 2,
												SouthEast = 3
								}

								enum HalfWind {
												NorthNorthEast = 0,
												EastNorthEast = 1,
												EastSouthEast = 2,
												SouthSouthEast = 3,
												SouthSouthWest = 4,
												WestSouthWest = 5,
												WestNorthWest = 6,
												NorthNorthWest = 7
								}

								#region Internal fields

								public bool isDirty;
								static CompassPro _instance;
								List<CompassActiveIcon> icons;
								float fadeStartTime, prevAlpha;
								CanvasGroup canvasGroup;
								RectTransform compassBackRect;
								Image compassBackImage;
								GameObject compassIconPrefab;
								Text text, textShadow;
								Text title, titleShadow;
								float endTimeOfCurrentTextReveal;
								Material spriteOverlayMat;
								Vector3 lastCamPos, lastCamRot;
								int frameCount;
								StringBuilder titleText;
								AudioSource audioSource;
								Text[] cardinals;
								Text[] ordinals;
								RawImage[] halfWinds;
								int poiVisibleCount;
								bool autoHiding;
								// performing autohide fade
								float thisAlpha;
								bool needUpdateBarContents;
								string lastDistanceText;
								float lastDistanceSqr;

								#endregion

								#region Gameloop lifecycle

								#if UNITY_EDITOR
		
								[MenuItem ("GameObject/UI/Compass Navigator Pro", false)]
								static void CreateCompassNavigatorPro (MenuCommand menuCommand) {
												// Create a custom game object
												GameObject go = Instantiate (Resources.Load<GameObject> ("CNPro/Prefabs/CompassNavigatorPro")) as GameObject;
												;
												go.name = "CompassNavigatorPro";
												GameObjectUtility.SetParentAndAlign (go, menuCommand.context as GameObject);
												Undo.RegisterCreatedObjectUndo (go, "Create " + go.name);
												Selection.activeObject = go;
								}

								[MenuItem ("GameObject/UI/Compass Navigator Pro", true)]
								static bool ValidateCreateCompassNavigatorPro (MenuCommand menuCommand) {
												return CompassPro.instance == null;
								}


								#endif
								public void OnEnable () {
												if (icons==null) Init();

												if (dontDestroyOnLoad && Application.isPlaying) {
																if (FindObjectsOfType (GetType ()).Length > 1) {
																				Destroy (gameObject);
																				return;
																}
																DontDestroyOnLoad (this);
												}
								}

								void Init() {
												icons = new List<CompassActiveIcon> (1000);
												audioSource = GetComponent<AudioSource> ();
												spriteOverlayMat = Resources.Load<Material> ("CNPro/Materials/SpriteOverlayUnlit");
												compassIconPrefab = Resources.Load<GameObject> ("CNPro/Prefabs/CompassIcon");
												canvasGroup = transform.GetComponent<CanvasGroup> ();
												GameObject compassBack = transform.Find ("CompassBack").gameObject;
												compassBackRect = compassBack.GetComponent<RectTransform> ();
												compassBackImage = compassBack.GetComponent<Image> ();
												text = compassBackRect.transform.Find ("Text").GetComponent<Text> ();
												textShadow = compassBackRect.transform.Find ("TextShadow").GetComponent<Text> ();
												text.text = textShadow.text = "";
												title = compassBackRect.transform.Find ("Title").GetComponent<Text> ();
												titleShadow = compassBackRect.transform.Find ("TitleShadow").GetComponent<Text> ();
												title.text = titleShadow.text = "";
												canvasGroup.alpha = 0;
												prevAlpha = 0f;
												fadeStartTime = Time.time;
												lastDistanceText = "";
												lastDistanceSqr = float.MinValue;
												cardinals = new Text[4];
												if (compassBackRect.transform.Find ("CardinalN") == null) {
																Debug.LogError ("CompassNavigatorPro gameobject has to be updated. Please delete and add prefab again to this scene.");
																_showCardinalPoints = false;
												} else {
																cardinals [(int)CardinalPoint.North] = compassBackRect.transform.Find ("CardinalN").GetComponent<Text> ();
																cardinals [(int)CardinalPoint.West] = compassBackRect.transform.Find ("CardinalW").GetComponent<Text> ();
																cardinals [(int)CardinalPoint.South] = compassBackRect.transform.Find ("CardinalS").GetComponent<Text> ();
																cardinals [(int)CardinalPoint.East] = compassBackRect.transform.Find ("CardinalE").GetComponent<Text> ();
												}
												ordinals = new Text[4];
												if (compassBackRect.transform.Find ("InterCardinalNW") == null) {
																Debug.LogError ("CompassNavigatorPro gameobject has to be updated. Please delete and add prefab again to this scene.");
																_showOrdinalPoints = false;
												} else {
																ordinals [(int)OrdinalPoint.NorthWest] = compassBackRect.transform.Find ("InterCardinalNW").GetComponent<Text> ();
																ordinals [(int)OrdinalPoint.NorthEast] = compassBackRect.transform.Find ("InterCardinalNE").GetComponent<Text> ();
																ordinals [(int)OrdinalPoint.SouthWest] = compassBackRect.transform.Find ("InterCardinalSW").GetComponent<Text> ();
																ordinals [(int)OrdinalPoint.SouthEast] = compassBackRect.transform.Find ("InterCardinalSE").GetComponent<Text> ();
												}
												halfWinds = new RawImage[8];
												if (compassBackRect.transform.Find ("TickNNE") == null) {
																Debug.LogError ("CompassNavigatorPro gameobject has to be updated. Please delete and add prefab again to this scene.");
																_showHalfWinds = false;
												} else {
																halfWinds [(int)HalfWind.EastNorthEast] = compassBackRect.transform.Find ("TickENE").GetComponent<RawImage> ();
																halfWinds [(int)HalfWind.EastSouthEast] = compassBackRect.transform.Find ("TickESE").GetComponent<RawImage> ();
																halfWinds [(int)HalfWind.NorthNorthEast] = compassBackRect.transform.Find ("TickNNE").GetComponent<RawImage> ();
																halfWinds [(int)HalfWind.NorthNorthWest] = compassBackRect.transform.Find ("TickNNW").GetComponent<RawImage> ();
																halfWinds [(int)HalfWind.SouthSouthEast] = compassBackRect.transform.Find ("TickSSE").GetComponent<RawImage> ();
																halfWinds [(int)HalfWind.SouthSouthWest] = compassBackRect.transform.Find ("TickSSW").GetComponent<RawImage> ();
																halfWinds [(int)HalfWind.WestNorthWest] = compassBackRect.transform.Find ("TickWNW").GetComponent<RawImage> ();
																halfWinds [(int)HalfWind.WestSouthWest] = compassBackRect.transform.Find ("TickWSW").GetComponent<RawImage> ();
												}

												UpdateCompassBarAppearance ();
												UpdateHalfWindsAppearance ();
												UpdateCompassBarAlpha ();
								}

								public void LateUpdate () {
												UpdateCompassBarAlpha ();
												UpdateCompassBarContents ();
								}

								#endregion

								#region Internal stuff

								/// <summary>
								/// Update bar icons
								/// </summary>
								void UpdateCompassBarContents () {

												if (!Application.isPlaying || Camera.main == null)
																return;

												// If camera has not moved, then don't refresh compass bar so often - just once each second in case one POI is moving
												if (++frameCount >= 60) {
																frameCount = 0;
																needUpdateBarContents = true;
												}
												if (!needUpdateBarContents) {
																if (lastCamPos != Camera.main.transform.position || lastCamRot != Camera.main.transform.eulerAngles) {
																				needUpdateBarContents = true;
																}
												}

												if (!needUpdateBarContents)
																return;

												needUpdateBarContents = false;
												lastCamPos = Camera.main.transform.position;
												lastCamRot = Camera.main.transform.eulerAngles;

												float visibleDistanceSQR = _visibleDistance * _visibleDistance;
												float visitedDistanceSQR = _visitedDistance * _visitedDistance;
												float nearDistanceSQR = _nearDistance * _nearDistance;
												float barMax = _width * 0.5f - _endCapsWidth / Camera.main.pixelWidth;
												const float visibleDistanceFallOffSQR = 25f * 25f;

												// Cardinal & Ordinal (ordinal) Points
												UpdateCardinalPoints (barMax);
												UpdateOrdinalPoints (barMax);
												UpdateHalfWinds (barMax);

												// Update Icons
												poiVisibleCount = 0;

												float nearestPOIDistanceThisFrame = float.MaxValue;
												CompassProPOI nearestPOI = null;

												for (int p = 0; p < icons.Count; p++) {
																bool iconVisible = false;
																CompassActiveIcon activeIcon = icons [p];
																CompassProPOI poi = activeIcon.poi;
																if (poi == null) {
																				if (activeIcon.rectTransform != null && activeIcon.rectTransform.gameObject != null) {
																								DestroyImmediate (activeIcon.rectTransform.gameObject);
																				}
																				icons.RemoveAt (p);	// POI no longer registered; remove and exit to prevent indexing errors
																				continue;
																}

																// Change in visibility?
																Vector3 poiPosition = poi.transform.position;
																float distanceSqr, distancePlanarSQR;
																distanceSqr = (poiPosition - lastCamPos).sqrMagnitude;
																distanceSqr -= poi.radius;
																if (distanceSqr <= 0)
																				distanceSqr = 0.01f;
																poi.distanceToCameraSQR = distanceSqr;

																// Distance has changed, proceed with update...
																if (_use3Ddistance) {
																				distancePlanarSQR = distanceSqr;
																} else {
																				Vector2 v = new Vector2 (poiPosition.x - lastCamPos.x, poiPosition.z - lastCamPos.z);
																				distancePlanarSQR = v.sqrMagnitude;
																}
																float factor = distancePlanarSQR / nearDistanceSQR;
																if (poi.showPlayModeGizmo) {
																				poi.iconAlpha = Mathf.Lerp (0.65f, 0, 5f * factor);
																}
																poi.iconScale = Misc.Vector3one * Mathf.Lerp (_maxIconSize, _minIconSize, factor);

																// Should we make this POI visible in the compass bar?
																if (poi.distanceToCameraSQR < visibleDistanceSQR && poi.isActiveAndEnabled) {
																				if (!poi.isVisible) {
																								poi.isVisible = true;
																								if (OnPOIVisible != null)
																												OnPOIVisible (poi);
																				}
																} else {
																				if (poi.isVisible) {
																								poi.isVisible = false;
																								if (OnPOIHide != null)
																												OnPOIHide (poi);
																				}
																}

																// Is it same scene?
																#if UNITY_5_4_OR_NEWER
																if (poi.isVisible && poi.dontDestroyOnLoad && !activeIcon.levelName.Equals (SceneManager.GetActiveScene ().name)) {
																				poi.isVisible = false;
																}
																#else
				if (poi.isVisible && poi.dontDestroyOnLoad && !activeIcon.levelName.Equals (Application.loadedLevelName)) {
					poi.isVisible = false;
				}
																#endif

																// Do we visit this POI for the first time?
																if (poi.isVisible && !poi.isVisited && poi.canBeVisited && poi.distanceToCameraSQR < visitedDistanceSQR) {
																				poi.isVisited = true;
																				if (OnPOIVisited != null)
																								OnPOIVisited (poi);
																				if (audioSource != null) {
																								if (poi.visitedAudioClip != null) {
																												audioSource.PlayOneShot (poi.visitedAudioClip);
																								} else if (_visitedDefaultAudioClip != null) {
																												audioSource.PlayOneShot (_visitedDefaultAudioClip);
																								}
																				}
																				ShowPOIDiscoveredText (poi);
																				if (poi.hideWhenVisited)
																								poi.enabled = false;
																}

																// Check heartbeat
																if (poi.heartbeatEnabled) {
																				bool inHeartbeatRange = poi.distanceToCameraSQR < poi.heartbeatDistance * poi.heartbeatDistance;
																				if (!poi.heartbeatIsActive && inHeartbeatRange) {
																								poi.StartHeartbeat ();
																				} else if (poi.heartbeatIsActive && !inHeartbeatRange) {
																								poi.StopHeartbeat ();
																				}
																}

																// If POI is not visible, then hide and skip
																if (!poi.isVisible) {
																				if (activeIcon.image != null && activeIcon.image.enabled) {
																								activeIcon.image.enabled = false;
																				}

																				if (poi.spriteRenderer != null && poi.spriteRenderer.enabled) {
																								poi.spriteRenderer.enabled = false;
																				}
																				continue;
																}

																// POI is visible, should we create the icon in the compass bar?
																if (activeIcon.rectTransform == null) {
																				GameObject iconGO = Instantiate (compassIconPrefab);
																				iconGO.hideFlags = HideFlags.DontSave;
																				iconGO.transform.SetParent (compassBackRect.transform, false);
																				activeIcon.rectTransform = iconGO.GetComponent<RectTransform> ();
																}

																// Position the icon on the compass bar
																Vector3 screenPos = GetScreenPos (poiPosition);
																float posX;
																if (Mathf.Abs (screenPos.x - activeIcon.lastPosX) > COMPASS_POI_POSITION_THRESHOLD) {
																				needUpdateBarContents = true;
																				posX = (screenPos.x + activeIcon.lastPosX) * 0.5f;
																} else {
																				posX = screenPos.x;
																}
																activeIcon.lastPosX = posX;

																// Always show the focused icon in the compass bar; if out of bar, maintain it on the edge with normal scale
																if (poi.clampPosition) {
																				if (screenPos.z < 0) {
																								posX = barMax * -Mathf.Sign (screenPos.x - 0.5f);
																								if (poi.iconScale.x > 1f)
																												poi.iconScale = Misc.Vector3one;
																				} else {
																								posX = Mathf.Clamp (posX, -barMax, barMax);
																								if (poi.iconScale.x > 1f)
																												poi.iconScale = Misc.Vector3one;
																				}
																				screenPos.z = 0;
																}
																float absPosX = Mathf.Abs (posX);

																// Set icon position
																if (absPosX > barMax || screenPos.z < 0) {
																				// Icon outside of bar
																				if (activeIcon.image != null && activeIcon.image.enabled) {
																								activeIcon.image.enabled = false;
																				}
																} else {
																				// Unhide icon
																				if (activeIcon.image != null && !activeIcon.image.enabled) {
																								activeIcon.image.enabled = true;
																								activeIcon.poi.visibleTime = Time.time;
																				}
																				activeIcon.rectTransform.anchorMin = activeIcon.rectTransform.anchorMax = new Vector2 (0.5f + posX / _width, 0.5f);
																				iconVisible = true;
																}

																// Icon is visible, manage it
																if (iconVisible) {
																				poiVisibleCount++;

																				// Check gizmo
																				if (!poi.showPlayModeGizmo && poi.spriteRenderer != null && poi.spriteRenderer.enabled) {
																								poi.spriteRenderer.enabled = false;
																				} else if (poi.showPlayModeGizmo) {
																								if (poi.spriteRenderer == null) {
																												// Add a dummy child gameObject
																												GameObject go = new GameObject ("POI Gizmo Renderer");
																												go.transform.SetParent (poi.transform, false);
																												poi.spriteRenderer = go.gameObject.AddComponent<SpriteRenderer> ();
																												poi.spriteRenderer.material = spriteOverlayMat;
																												poi.spriteRenderer.enabled = false;
																								}
																								if (poi.spriteRenderer != null) {
																												if (!poi.spriteRenderer.enabled) {
																																poi.spriteRenderer.enabled = true;
																																poi.spriteRenderer.sprite = poi.iconVisited;
																												}
																												poi.spriteRenderer.transform.LookAt (lastCamPos);
																												poi.spriteRenderer.transform.localScale = Misc.Vector3one * _gizmoScale;
																												poi.spriteRenderer.color = new Color (1, 1, 1, poi.iconAlpha);
																								}
																				}

																				// Assign proper icon
																				if (activeIcon.poi.isVisited) {
																								if (activeIcon.image != poi.iconVisited) {
																												activeIcon.image.sprite = poi.iconVisited;
																								}
																				} else if (activeIcon.image != poi.iconNonVisited) {
																								activeIcon.image.sprite = poi.iconNonVisited;
																				}

																				// Scale in animation
																				if (_scaleInDuration > 0) {
																								float t = (Time.time - activeIcon.poi.visibleTime) / _scaleInDuration;
																								if (t < 1) {
																												needUpdateBarContents = true;
																												activeIcon.poi.iconScale *= t;
																								}
																				}

																				// Scale icon
																				if (activeIcon.poi.iconScale != activeIcon.rectTransform.localScale) {
																								activeIcon.rectTransform.localScale = activeIcon.poi.iconScale;
																				}

																				// Set icon's alpha
																				if (visibleDistanceFallOffSQR > 0) {
																								Color spriteColor = activeIcon.image.color;
																								float t = (visibleDistanceSQR - poi.distanceToCameraSQR) / visibleDistanceFallOffSQR;
																								spriteColor.a = Mathf.Lerp (0, 1, t);
																								activeIcon.image.color = spriteColor;
																				}

																				// Get title if POI is centered
																				if (absPosX < _labelHotZone && distancePlanarSQR < nearestPOIDistanceThisFrame) {
																								nearestPOI = poi;
																								nearestPOIDistanceThisFrame = distancePlanarSQR;
																				}
																}

												}

												// Update title
												if (nearestPOI != null) {
																if (titleText == null) {
																				titleText = new StringBuilder ();
																} else {
																				if (titleText.Length > 0)
																								titleText.Length = 0;
																}
																if (nearestPOI.isVisited || nearestPOI.titleVisibility == TITLE_VISIBILITY.Always) {
																				titleText.Append (nearestPOI.title);
																}
																// indicate "above" or "below"
																bool addedAlt = false; 
																if (nearestPOI.transform.position.y > lastCamPos.y + _sameAltitudeThreshold) {
																				if (titleText.Length > 0)
																								titleText.Append (" ");
																				titleText.Append ("(Above");
																				addedAlt = true;
																} else if (nearestPOI.transform.position.y < lastCamPos.y - _sameAltitudeThreshold) {
																				if (titleText.Length > 0)
																								titleText.Append (" ");
																				titleText.Append ("(Below");
																				addedAlt = true;
																}
																if (_showDistance) {
																				if (addedAlt) {
																								titleText.Append (", ");
																				} else {
																								if (titleText.Length > 0)
																												titleText.Append (" ");
																								titleText.Append ("(");
																				}
																				if (lastDistanceSqr != nearestPOIDistanceThisFrame) {
																								lastDistanceSqr = nearestPOIDistanceThisFrame;
																								lastDistanceText = Mathf.Sqrt (nearestPOIDistanceThisFrame).ToString (showDistanceFormat);
																				}
																				titleText.Append (lastDistanceText);
																				titleText.Append (" m)");
																} else if (addedAlt) {
																				titleText.Append (")");
																}

																string tt = titleText.ToString ();
																if (!title.text.Equals (tt)) {
																				title.text = titleShadow.text = tt;
																				UpdateTitleAlpha (1.0f);
																				UpdateTitleAppearance ();
																}
												} else {
																title.text = titleShadow.text = "";
												}


								}

								Vector3 GetScreenPos (Vector3 poiPosition) {
												Vector3 camPos = Camera.main.transform.position;
												poiPosition.y = camPos.y;
												Vector3 screenPos = Misc.Vector3zero;
												switch (_worldMappingMode) {
												case WORLD_MAPPING_MODE.LimitedToBarWidth: 
																screenPos = Camera.main.WorldToViewportPoint (poiPosition);
																break;
												case WORLD_MAPPING_MODE.Full180Degrees:
																{
																				Vector3 v2poi = poiPosition - camPos;
																				Vector3 dir = new Vector3 (Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z);
																				float angle = (Quaternion.FromToRotation (dir, v2poi).eulerAngles.y + 180f) / 180f;
																				screenPos.x = 0.5f + (angle % 2.0f - 1.0f) * (_width - _endCapsWidth / Camera.main.pixelWidth) * 0.9f;
																}
																break;
												case WORLD_MAPPING_MODE.Full360Degrees: 
																{
																				Vector3 v2poi = poiPosition - camPos;
																				Vector3 dir = new Vector3 (Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z);
																				float angle = (Quaternion.FromToRotation (dir, v2poi).eulerAngles.y + 180f) / 180f;
																				screenPos.x = 0.5f + (angle % 2.0f - 1f) * 0.5f * (_width - _endCapsWidth / Camera.main.pixelWidth) * 0.9f;
																}
																break;
												default: // WORLD_MAPPING_MODE.CameraFustrum: 
																screenPos = Camera.main.WorldToViewportPoint (poiPosition);
																screenPos.x = 0.5f + (screenPos.x - 0.5f) * (_width - _endCapsWidth / Camera.main.pixelWidth) * 0.9f;
																break;
												}
												screenPos.x -= 0.5f;
												return screenPos;

								}

								/// <summary>
								/// If showCardinalPoints is enabled, show N, W, S, E across the compass bar
								/// </summary>
								void UpdateCardinalPoints (float barMax) {

												for (int k = 0; k < cardinals.Length; k++) {
																if (!_showCardinalPoints) {
																				if (cardinals [k].enabled) {
																								cardinals [k].enabled = false;
																				}
																				continue;
																}
																Vector3 cardinalPointWorldPosition = Camera.main.transform.position;
																switch (k) {
																case (int)CardinalPoint.North:
																				cardinalPointWorldPosition.z += 1f;
																				break;
																case (int)CardinalPoint.West:
																				cardinalPointWorldPosition.x -= 1f;
																				break;
																case (int)CardinalPoint.South:
																				cardinalPointWorldPosition.z -= 1f;
																				break;
																case (int)CardinalPoint.East:
																				cardinalPointWorldPosition.x += 1f;
																				break;
																}
																Vector3 screenPos = GetScreenPos (cardinalPointWorldPosition);
																float posX = screenPos.x;
																float absPosX = Mathf.Abs (posX);

																// Set icon position
																if (absPosX > barMax || screenPos.z <= 0) {
																				// Icon outside of bar
																				if (cardinals [k].enabled) {
																								cardinals [k].enabled = false;
																				}
																} else {
																				// Unhide icon
																				if (!cardinals [k].enabled) {
																								cardinals [k].enabled = true;
																				}
																				RectTransform rt = cardinals [k].rectTransform;
																				rt.anchorMin = rt.anchorMax = new Vector2 (0.5f + posX / _width, 0.5f);
																}
												}

								}


								/// <summary>
								/// If showOrdinalPoints is enabled, show NE, NW, SW, SE across the compass bar
								/// </summary>
								void UpdateOrdinalPoints (float barMax) {
			
												for (int k = 0; k < ordinals.Length; k++) {
																if (!_showOrdinalPoints) {
																				if (ordinals [k].enabled) {
																								ordinals [k].enabled = false;
																				}
																				continue;
																}
																Vector3 ordinalPointWorldPosition = Camera.main.transform.position;
																switch (k) {
																case (int)OrdinalPoint.NorthWest:
																				ordinalPointWorldPosition.x -= 1f;
																				ordinalPointWorldPosition.z += 1f;
																				break;
																case (int)OrdinalPoint.NorthEast:
																				ordinalPointWorldPosition.x += 1f;
																				ordinalPointWorldPosition.z += 1f;
																				break;
																case (int)OrdinalPoint.SouthWest:
																				ordinalPointWorldPosition.x -= 1f;
																				ordinalPointWorldPosition.z -= 1f;
																				break;
																case (int)OrdinalPoint.SouthEast:
																				ordinalPointWorldPosition.x += 1f;
																				ordinalPointWorldPosition.z -= 1f;
																				break;
																}
																Vector3 screenPos = GetScreenPos (ordinalPointWorldPosition); // Camera.main.WorldToViewportPoint (ordinalPointWorldPosition);
																float posX = screenPos.x; 
																float absPosX = Mathf.Abs (posX);
				
																// Set icon position
																if (absPosX > barMax || screenPos.z <= 0) {
																				// Icon outside of bar
																				if (ordinals [k].enabled) {
																								ordinals [k].enabled = false;
																				}
																} else {
																				// Unhide icon
																				if (!ordinals [k].enabled) {
																								ordinals [k].enabled = true;
																				}
																				ordinals [k].rectTransform.anchorMin = ordinals [k].rectTransform.anchorMax = new Vector2 (0.5f + posX / _width, 0.5f);
																}
												}
			
								}


								/// <summary>
								/// If showHalfWinds is enabled, show NNE, ENE, ESE, SSE, SSW, WSW, WNW, NNW marks
								/// </summary>
								void UpdateHalfWinds (float barMax) {
												const float h4 = 0.4f; 
												const float h8 = 1f;

												for (int k = 0; k < halfWinds.Length; k++) {
																if (!_showHalfWinds) {
																				if (halfWinds [k].enabled) {
																								halfWinds [k].enabled = false;
																				}
																				continue;
																}
																Vector3 halfWindPointWorldPosition = Camera.main.transform.position;
																switch (k) {
																case (int)HalfWind.EastNorthEast:
																				halfWindPointWorldPosition.x += h8;
																				halfWindPointWorldPosition.z += h4;
																				break;
																case (int)HalfWind.EastSouthEast:
																				halfWindPointWorldPosition.x += h8;
																				halfWindPointWorldPosition.z -= h4;
																				break;
																case (int)HalfWind.NorthNorthEast:
																				halfWindPointWorldPosition.x += h4;
																				halfWindPointWorldPosition.z += h8;
																				break;
																case (int)HalfWind.NorthNorthWest:
																				halfWindPointWorldPosition.x -= h4;
																				halfWindPointWorldPosition.z += h8;
																				break;
																case (int)HalfWind.SouthSouthEast:
																				halfWindPointWorldPosition.x += h4;
																				halfWindPointWorldPosition.z -= h8;
																				break;
																case (int)HalfWind.SouthSouthWest:
																				halfWindPointWorldPosition.x -= h4;
																				halfWindPointWorldPosition.z -= h8;
																				break;
																case (int)HalfWind.WestNorthWest:
																				halfWindPointWorldPosition.x -= h8;
																				halfWindPointWorldPosition.z += h4;
																				break;
																case (int)HalfWind.WestSouthWest:
																				halfWindPointWorldPosition.x -= h8;
																				halfWindPointWorldPosition.z -= h4;
																				break;
																}
																Vector3 screenPos = GetScreenPos (halfWindPointWorldPosition);
																float posX = screenPos.x;
																float absPosX = Mathf.Abs (posX);
				
																// Set icon position
																if (absPosX > barMax || screenPos.z <= 0) {
																				// Icon outside of bar
																				if (halfWinds [k].enabled) {
																								halfWinds [k].enabled = false;
																				}
																} else {
																				// Unhide icon
																				if (!halfWinds [k].enabled) {
																								halfWinds [k].enabled = true;
																				}
																				halfWinds [k].rectTransform.anchorMin = new Vector2 (0.5f + posX / _width, 0f);
																				halfWinds [k].rectTransform.anchorMax = new Vector2 (0.5f + posX / _width, 1f);
																}
												}
			
								}


								/// <summary>
								/// Manages compass bar alpha transitions
								/// </summary>
								void UpdateCompassBarAlpha () {

												// Alpha
												if (_alwaysVisibleInEditMode && !Application.isPlaying) {
																thisAlpha = 1.0f;
												} else if (_autoHide) {
																if (!autoHiding) {
																				if (poiVisibleCount == 0) {
																								if (thisAlpha > 0) {
																												autoHiding = true;
																												fadeStartTime = Time.time;
																												prevAlpha = canvasGroup.alpha;
																												thisAlpha = 0;
																								}
																				} else if (poiVisibleCount > 0 && thisAlpha == 0) {
																								thisAlpha = _alpha;
																								autoHiding = true;
																								fadeStartTime = Time.time;
																								prevAlpha = canvasGroup.alpha;
																				}
																}
												} else {
																thisAlpha = _alpha;
												}

												if (thisAlpha != canvasGroup.alpha) {
																float t = Application.isPlaying ? (Time.time - fadeStartTime) / _fadeDuration : 1.0f;
																canvasGroup.alpha = Mathf.Lerp (prevAlpha, thisAlpha, t);
																if (t >= 1) {
																				prevAlpha = canvasGroup.alpha;
																}
												} else if (autoHiding)
																autoHiding = false;
								}

								void UpdateCompassBarAppearance () {
												// Width & Vertical Position
												float anchorMinX = (1 - _width) * 0.5f;
												compassBackRect.anchorMin = new Vector2 (anchorMinX, _verticalPosition);
												float anchorMaxX = 1f - anchorMinX;
												compassBackRect.anchorMax = new Vector2 (anchorMaxX, _verticalPosition);

												// Style
												Sprite barSprite;
												switch (_style) {
												case COMPASS_STYLE.Rounded:
																barSprite = Resources.Load<Sprite> ("CNPro/Sprites/Bar2");
																break;
												case COMPASS_STYLE.Celtic_White:
																barSprite = Resources.Load<Sprite> ("CNPro/Sprites/Bar3-White");
																break;
												case COMPASS_STYLE.Celtic_Black:
																barSprite = Resources.Load<Sprite> ("CNPro/Sprites/Bar3-Black");
																break;
												default:
																barSprite = Resources.Load<Sprite> ("CNPro/Sprites/Bar1");
																break;
												}
												if (compassBackImage.sprite != barSprite) {
																compassBackImage.sprite = barSprite;
												}

								}

								void UpdateHalfWindsAppearance () {
												if (halfWinds == null)
																return;
												for (int k = 0; k < halfWinds.Length; k++) {
																halfWinds [k].enabled = _showHalfWinds;
																halfWinds [k].color = _halfWindsTintColor;
																halfWinds [k].transform.localScale = new Vector3 (_halfWindsWidth, _halfWindsHeight, 1f);
												}
								}

								void UpdateTextAppearanceEditMode () {
												if (!gameObject.activeInHierarchy)
																return;
												text.text = textShadow.text = "SAMPLE TEXT";
												UpdateTextAlpha (1);
												UpdateTextAppearance (text.text);
								}

								void UpdateTextAppearance (string sizeText) {
												// Vertical and horizontal position
												text.GetComponent<RectTransform> ().anchoredPosition3D = new Vector3 (0, _textVerticalPosition, 0);
												text.transform.localScale = Misc.Vector3one * _textScale;
												text.font = textFont;
												textShadow.GetComponent<RectTransform> ().anchoredPosition3D = new Vector3 (1f, _textVerticalPosition - 1f, 0);
												textShadow.transform.localScale = Misc.Vector3one * _textScale;
								}

								void UpdateTextAlpha (float t) {
												text.color = new Color (text.color.r, text.color.g, text.color.b, t);
												textShadow.color = new Color (0, 0, 0, t);
								}

								void UpdateTitleAppearanceEditMode () {
												if (!gameObject.activeInHierarchy)
																return;
												title.text = titleShadow.text = "SAMPLE TITLE";
												UpdateTitleAlpha (1);
												UpdateTitleAppearance ();
								}

								void UpdateTitleAppearance () {
												// Vertical and horizontal position
												title.GetComponent<RectTransform> ().anchoredPosition3D = new Vector3 (0, _titleVerticalPosition, 0);
												title.transform.localScale = Misc.Vector3one * _titleScale;
												title.font = titleFont;
												titleShadow.GetComponent<RectTransform> ().anchoredPosition3D = new Vector3 (1f, _titleVerticalPosition - 1f, 0);
												titleShadow.transform.localScale = Misc.Vector3one * _titleScale;
								}

								void UpdateTitleAlpha (float t) {
												title.color = new Color (title.color.r, title.color.g, title.color.b, t);
												titleShadow.color = new Color (0, 0, 0, t);
								}

								void ShowPOIDiscoveredText (CompassProPOI poi) {
												if (poi.visitedText == null || !_textRevealEnabled)
																return;
												StartCoroutine (AnimateDiscoverText (poi.visitedText));
								}

								IEnumerator AnimateDiscoverText (string discoverText) {

												int len = discoverText.Length;
												if (len == 0 || Camera.main == null)
																yield break;

												while (Time.time < endTimeOfCurrentTextReveal) {
																yield return new WaitForSeconds (1);
												}

												text.text = textShadow.text = "";

												float now = Time.time;
												endTimeOfCurrentTextReveal = now + _textRevealDuration + _textDuration + _textFadeOutDuration * 0.5f;
												UpdateTextAppearance (discoverText);

												// initial pos of text
												string discoverTextSpread = discoverText.Replace (" ", "A");
												float posX = -text.cachedTextGenerator.GetPreferredWidth (discoverTextSpread, text.GetGenerationSettings (Misc.Vector2zero)) * 0.5f * _textScale;

												float acum = 0;
												for (int k = 0; k < len; k++) {
																string ch = discoverText.Substring (k, 1);

																// Setup shadow (first, so it goes behind white text)
																GameObject letterShadow = Instantiate (textShadow.gameObject);
																letterShadow.transform.SetParent (text.transform.parent, false);
																Text lts = letterShadow.GetComponent<Text> ();
																lts.text = ch;

																// Setup letter
																GameObject letter = Instantiate (text.gameObject);
																letter.transform.SetParent (text.transform.parent, false);
																Text lt = letter.GetComponent<Text> ();
																lt.text = ch;

																float letw = 0;
																if (ch.Equals (" ")) {
																				letw = lt.cachedTextGenerator.GetPreferredWidth ("A", lt.GetGenerationSettings (Misc.Vector2zero)) * _textScale;
																} else {
																				letw = lt.cachedTextGenerator.GetPreferredWidth (ch, lt.GetGenerationSettings (Misc.Vector2zero)) * _textScale;
																}

																RectTransform letterRT = letter.GetComponent<RectTransform> ();
																letterRT.anchoredPosition3D = new Vector3 (posX + acum + letw * 0.5f, letterRT.anchoredPosition3D.y, 0);
																RectTransform shadowRT = letterShadow.GetComponent<RectTransform> ();
																shadowRT.anchoredPosition3D = new Vector3 (posX + acum + letw * 0.5f + 1f, shadowRT.anchoredPosition3D.y, 0);

																acum += letw;

																// Trigger animator
																LetterAnimator anim = letterShadow.AddComponent<LetterAnimator> ();
																anim.text = lt;
																anim.textShadow = lts;
																anim.startTime = now + k * _textRevealLetterDelay;
																anim.revealDuration = _textRevealDuration;
																anim.startFadeTime = now + _textRevealDuration + _textDuration;
																anim.fadeDuration = _textFadeOutDuration;
												}
								}

								#endregion
	
				}



}



