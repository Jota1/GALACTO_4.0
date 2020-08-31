using UnityEngine;
using UnityEditor;
using System.Collections;

namespace CompassNavigatorPro {
				[CustomEditor (typeof(CompassPro))]
				public class CompassNavigatorProInspector : Editor {

								CompassPro _compass;
								Texture2D _headerTexture, _blackTexture;
								GUIStyle blackStyle, sectionHeaderStyle;
								Color titleColor;

								void OnEnable () {
												Color backColor = EditorGUIUtility.isProSkin ? new Color (0.18f, 0.18f, 0.18f) : new Color (0.7f, 0.7f, 0.7f);
												titleColor = EditorGUIUtility.isProSkin ? new Color (0.52f, 0.66f, 0.9f) : new Color (0.12f, 0.16f, 0.4f);
												_blackTexture = MakeTex (4, 4, backColor);
												_blackTexture.hideFlags = HideFlags.DontSave;
												blackStyle = new GUIStyle ();
												blackStyle.normal.background = _blackTexture;
												_compass = (CompassPro)target;
												_headerTexture = Resources.Load<Texture2D> ("CNPro/CompassNavigatorProHeader");

								}

								public override void OnInspectorGUI () {
												if (_compass == null)
																return;
												_compass.isDirty = false;

												EditorGUILayout.Separator ();
												GUI.skin.label.alignment = TextAnchor.MiddleCenter;  
												GUILayout.Label (_headerTexture, GUILayout.ExpandWidth (true));
												GUI.skin.label.alignment = TextAnchor.MiddleLeft;  

												EditorGUILayout.BeginVertical (blackStyle);
			
												EditorGUILayout.BeginHorizontal ();
												DrawTitleLabel ("Compass Bar Settings");
												if (GUILayout.Button ("Help", GUILayout.Width (50)))
																EditorUtility.DisplayDialog ("Help", "Move the mouse over each label to show a description of the parameter.", "Ok");
												if (GUILayout.Button ("About", GUILayout.Width (60))) {
																CompassProAbout.ShowAboutWindow ();
												}
												EditorGUILayout.EndHorizontal ();

												EditorGUILayout.BeginHorizontal ();
												GUILayout.Label (new GUIContent ("Style", "Compass bar style."), GUILayout.Width (130));
												_compass.style = (COMPASS_STYLE)EditorGUILayout.EnumPopup (_compass.style);
												EditorGUILayout.EndHorizontal ();

												EditorGUILayout.BeginHorizontal ();
												GUILayout.Label (new GUIContent ("Vertical Position", "Distance from the bottom of the screen in %."), GUILayout.Width (130));
												_compass.verticalPosition = EditorGUILayout.Slider (_compass.verticalPosition, -0.05f, 1.05f);
												EditorGUILayout.EndHorizontal ();

												EditorGUILayout.BeginHorizontal ();
												GUILayout.Label (new GUIContent ("Width", "Width of the compass bar in % of the screen width."), GUILayout.Width (130));
												_compass.width = EditorGUILayout.Slider (_compass.width, 0.05f, 1f);
												EditorGUILayout.EndHorizontal ();
			
												EditorGUILayout.BeginHorizontal ();
												GUILayout.Label (new GUIContent ("End Caps Width", "Width of the end caps of the compass bar. This setting limits the usable horizontal range of the bar in the screen to prevent icons being drawn over the art of the end caps of the bar."), GUILayout.Width (130));
												_compass.endCapsWidth = EditorGUILayout.Slider (_compass.endCapsWidth, 0, 100f);
												EditorGUILayout.EndHorizontal ();

												EditorGUILayout.BeginHorizontal ();
												GUILayout.Label (new GUIContent ("Alpha", "Transparency of the compass bar."), GUILayout.Width (130));
												_compass.alpha = EditorGUILayout.Slider (_compass.alpha, 0f, 1f);
												EditorGUILayout.EndHorizontal ();

												EditorGUILayout.BeginHorizontal ();
												GUILayout.Label (new GUIContent ("  Visible In Edit Mode", "Makes the bar always visible (ignored alpha property) while in Edit Mode."), GUILayout.Width (130));
												_compass.alwaysVisibleInEditMode = EditorGUILayout.Toggle (_compass.alwaysVisibleInEditMode);
												EditorGUILayout.EndHorizontal ();

												EditorGUILayout.BeginHorizontal ();
												GUILayout.Label (new GUIContent ("Auto Hide If Empty", "Hides the compass bar if no POIs are below visible distance."), GUILayout.Width (130));
												_compass.autoHide = EditorGUILayout.Toggle (_compass.autoHide);
												EditorGUILayout.EndHorizontal ();

												EditorGUILayout.BeginHorizontal ();
												GUILayout.Label (new GUIContent ("Fade Duration", "Duration of alpha changes in seconds."), GUILayout.Width (130));
												_compass.fadeDuration = EditorGUILayout.Slider (_compass.fadeDuration, 0f, 8f);
												EditorGUILayout.EndHorizontal ();

												EditorGUILayout.BeginHorizontal ();
												GUILayout.Label (new GUIContent ("World Mapping Mode", "How POIs positions are mapped to the bar. 1) Limited To Bar Width = the bar width determines the view angle, 2) Camera Frustum = the entire camera frustum is mapped to the bar width, 3) Full 180 degrees = all POIs in front of the camera will appear in the compass bar. 4) Full 360 degrees = all POIs are visible in the compass bar."), GUILayout.Width (130));
												_compass.worldMappingMode = (WORLD_MAPPING_MODE)EditorGUILayout.EnumPopup (_compass.worldMappingMode);
												EditorGUILayout.EndHorizontal ();

												EditorGUILayout.BeginHorizontal ();
												GUILayout.Label (new GUIContent ("Use 3D Distance", "Whether 3D distance should be computed instead of planar X/Z distance"), GUILayout.Width (130));
												_compass.use3Ddistance = EditorGUILayout.Toggle (_compass.use3Ddistance);
												EditorGUILayout.EndHorizontal ();
			
												EditorGUILayout.BeginHorizontal ();
												GUILayout.Label (new GUIContent ("Same Altitude Diff.", "Minimum difference in altitude from camera to show 'above' or 'below'"), GUILayout.Width (130));
												_compass.sameAltitudeThreshold = EditorGUILayout.Slider (_compass.sameAltitudeThreshold, 1f, 50f);
												EditorGUILayout.EndHorizontal ();

												EditorGUILayout.BeginHorizontal ();
												GUILayout.Label (new GUIContent ("Show Cardinal Points", "Whether N, W, S, E should be visible in the compass bar."), GUILayout.Width (130));
												_compass.showCardinalPoints = EditorGUILayout.Toggle (_compass.showCardinalPoints);
												EditorGUILayout.EndHorizontal ();

												EditorGUILayout.BeginHorizontal ();
												GUILayout.Label (new GUIContent ("Show Ordinal Points", "Whether NW, NE, SW, SE should be visible in the compass bar."), GUILayout.Width (130));
												_compass.showOrdinalPoints = EditorGUILayout.Toggle (_compass.showOrdinalPoints);
												EditorGUILayout.EndHorizontal ();

												EditorGUILayout.BeginHorizontal ();
												GUILayout.Label (new GUIContent ("Show Half Winds", "Enable vertical interval marks in the compass bar."), GUILayout.Width (130));
												_compass.showHalfWinds = EditorGUILayout.Toggle (_compass.showHalfWinds);
												EditorGUILayout.EndHorizontal ();
												if (_compass.showHalfWinds) {
																EditorGUILayout.BeginHorizontal ();
																GUILayout.Label ("  Height", GUILayout.Width (130));
																_compass.halfWindsHeight = EditorGUILayout.Slider (_compass.halfWindsHeight, 0.1f, 1f);
																EditorGUILayout.EndHorizontal ();
																EditorGUILayout.BeginHorizontal ();
																GUILayout.Label ("  Width", GUILayout.Width (130));
																_compass.halfWindsWidth = EditorGUILayout.Slider (_compass.halfWindsWidth, 1f, 5f);
																EditorGUILayout.EndHorizontal ();
																EditorGUILayout.BeginHorizontal ();
																GUILayout.Label ("  Tint Color", GUILayout.Width (130));
																_compass.halfWindsTintColor = EditorGUILayout.ColorField (_compass.halfWindsTintColor);
																EditorGUILayout.EndHorizontal ();

												}

												EditorGUILayout.BeginHorizontal ();
												GUILayout.Label (new GUIContent ("Show Distance (meters)", "Whether the distance in meters should be shown in the title."), GUILayout.Width (130));
												_compass.showDistance = EditorGUILayout.Toggle (_compass.showDistance);
												EditorGUILayout.EndHorizontal ();

												if (_compass.showDistance) {
																EditorGUILayout.BeginHorizontal ();
																GUILayout.Label (new GUIContent ("  String Format", " The string format for displaying the distance. A value of F0 means 'Fixed/Decimal with 0 decimal positions'. A value of F1 includes 1 decimal position. The sintax for this string format corresponds with the available options for ToString(format) method of C#."), GUILayout.Width (130));
																_compass.showDistanceFormat = EditorGUILayout.TextField (_compass.showDistanceFormat);
																EditorGUILayout.EndHorizontal ();
												}

												EditorGUILayout.BeginHorizontal ();
												GUILayout.Label (new GUIContent ("Don't Destroy On Load", "Preserve compass bar between scene changes."), GUILayout.Width (130));
												_compass.dontDestroyOnLoad = EditorGUILayout.Toggle (_compass.dontDestroyOnLoad);
												EditorGUILayout.EndHorizontal ();

												EditorGUILayout.EndVertical ();
												EditorGUILayout.Separator ();

												EditorGUILayout.BeginVertical (blackStyle);
			
												EditorGUILayout.BeginHorizontal ();
												DrawTitleLabel ("POIs Settings");
												EditorGUILayout.EndHorizontal ();
			
												EditorGUILayout.BeginHorizontal ();
												GUILayout.Label (new GUIContent ("Visible Distance", "POIs beyond visible distance (meters) will not be shown in the compass bar."), GUILayout.Width (130));
												_compass.visibleDistance = EditorGUILayout.Slider (_compass.visibleDistance, 10, 10000);
												EditorGUILayout.EndHorizontal ();

												EditorGUILayout.BeginHorizontal ();
												GUILayout.Label (new GUIContent ("Near Distance", "Distance to a POI where the icon will start to grow as player approaches."), GUILayout.Width (130));
												_compass.nearDistance = EditorGUILayout.Slider (_compass.nearDistance, 10, 10000);
												EditorGUILayout.EndHorizontal ();
			
												EditorGUILayout.BeginHorizontal ();
												GUILayout.Label (new GUIContent ("Visited Distance", "Minimum distance to a POI to be considered as explored/visited."), GUILayout.Width (130));
												_compass.visitedDistance = EditorGUILayout.Slider (_compass.visitedDistance, 1, 10000);
												EditorGUILayout.EndHorizontal ();

												EditorGUILayout.BeginHorizontal ();
												GUILayout.Label (new GUIContent ("Icon Size Range", "Minimum and maximum icon sizes. Icons grow/shrink depending on distance."), GUILayout.Width (130));
												float minIconSize = _compass.minIconSize;
												float maxIconSize = _compass.maxIconSize;
												EditorGUILayout.MinMaxSlider (ref minIconSize, ref maxIconSize, 0.1f, 2f);
												_compass.minIconSize = minIconSize;
												_compass.maxIconSize = maxIconSize;
												GUILayout.Label (minIconSize.ToString ("F2") + "-" + maxIconSize.ToString ("F2"));
												EditorGUILayout.EndHorizontal ();

												EditorGUILayout.BeginHorizontal ();
												GUILayout.Label (new GUIContent ("Scale In Duration", "Duration for the scale animation when the POI appears on the compass bar."), GUILayout.Width (130));
												_compass.scaleInDuration = EditorGUILayout.Slider (_compass.scaleInDuration, 0, 5);
												EditorGUILayout.EndHorizontal ();

												EditorGUILayout.BeginHorizontal ();
												GUILayout.Label (new GUIContent ("Label Hot Zone", "The distance from the center of the compass bar where a POI's label is visible."), GUILayout.Width (130));
												_compass.labelHotZone = EditorGUILayout.Slider (_compass.labelHotZone, 0.001f, 0.2f);
												EditorGUILayout.EndHorizontal ();

												EditorGUILayout.BeginHorizontal ();
												GUILayout.Label (new GUIContent ("Gizmo Scale", "Scaling applied to gizmos shown during playmode."), GUILayout.Width (130));
												_compass.gizmoScale = EditorGUILayout.Slider (_compass.gizmoScale, 0.01f, 1f);
												EditorGUILayout.EndHorizontal ();
			
												EditorGUILayout.BeginHorizontal ();
												GUILayout.Label (new GUIContent ("Visited Sound", "Default audio clip to be played when a POI is visited for the first time. Note that you can specify a different audio clip in the POI script itself."), GUILayout.Width (130));
												_compass.visitedDefaultAudioClip = (AudioClip)EditorGUILayout.ObjectField (_compass.visitedDefaultAudioClip, typeof(AudioClip), false);
												EditorGUILayout.EndHorizontal ();

												EditorGUILayout.BeginHorizontal ();
												GUILayout.Label (new GUIContent ("Beacon Sound", "Default audio clip to be played when a POI beacon is shown. Note that you can specify a different audio clip in the POI script itself."), GUILayout.Width (130));
												_compass.beaconDefaultAudioClip = (AudioClip)EditorGUILayout.ObjectField (_compass.beaconDefaultAudioClip, typeof(AudioClip), false);
												EditorGUILayout.EndHorizontal ();
			
												EditorGUILayout.BeginHorizontal ();
												GUILayout.Label (new GUIContent ("Heartbeat Sound", "Default audio clip to play for the heartbeat effect. This effect is enabled on each POI and will play a custom sound with variable speed depending on distance."), GUILayout.Width (130));
												_compass.heartbeatDefaultAudioClip = (AudioClip)EditorGUILayout.ObjectField (_compass.heartbeatDefaultAudioClip, typeof(AudioClip), false);
												EditorGUILayout.EndHorizontal ();

												EditorGUILayout.EndVertical ();

												EditorGUILayout.Separator ();
			
												EditorGUILayout.BeginVertical (blackStyle);
			
												EditorGUILayout.BeginHorizontal ();
												DrawTitleLabel ("Title Settings");
												EditorGUILayout.EndHorizontal ();

												EditorGUILayout.BeginHorizontal ();
												GUILayout.Label (new GUIContent ("Font", "Font for the title."), GUILayout.Width (130));
												_compass.titleFont = (Font)EditorGUILayout.ObjectField (_compass.titleFont, typeof(Font), false);
												EditorGUILayout.EndHorizontal ();

												EditorGUILayout.BeginHorizontal ();
												GUILayout.Label (new GUIContent ("Vertical Offset", "Vertical offset in pixels for the title with respect to the compass bar."), GUILayout.Width (130));
												_compass.titleVerticalPosition = EditorGUILayout.Slider (_compass.titleVerticalPosition, -200, 200);
												EditorGUILayout.EndHorizontal ();

												EditorGUILayout.BeginHorizontal ();
												GUILayout.Label (new GUIContent ("Scale", "Scaling applied to the title."), GUILayout.Width (130));
												_compass.titleScale = EditorGUILayout.Slider (_compass.titleScale, 0.02f, 3);
												EditorGUILayout.EndHorizontal ();

												EditorGUILayout.EndVertical ();
												EditorGUILayout.Separator ();

												EditorGUILayout.BeginVertical (blackStyle);
												EditorGUILayout.BeginHorizontal ();
												DrawTitleLabel ("Text Settings");
												EditorGUILayout.EndHorizontal ();
			
												EditorGUILayout.BeginHorizontal ();
												GUILayout.Label (new GUIContent ("Enable", "Show a revealing text effect when discovering POIs for the first time."), GUILayout.Width (130));
												_compass.textRevealEnabled = EditorGUILayout.Toggle (_compass.textRevealEnabled);
												EditorGUILayout.EndHorizontal ();

												EditorGUILayout.BeginHorizontal ();
												GUILayout.Label (new GUIContent ("Font", "Font for the text."), GUILayout.Width (130));
												_compass.textFont = (Font)EditorGUILayout.ObjectField (_compass.textFont, typeof(Font), false);
												EditorGUILayout.EndHorizontal ();

												EditorGUILayout.BeginHorizontal ();
												GUILayout.Label (new GUIContent ("Vertical Offset", "Vertical offset in pixels for the text with respect to the compass bar."), GUILayout.Width (130));
												_compass.textVerticalPosition = EditorGUILayout.Slider (_compass.textVerticalPosition, -200, 200);
												EditorGUILayout.EndHorizontal ();
			
												EditorGUILayout.BeginHorizontal ();
												GUILayout.Label (new GUIContent ("Scale", "Scaling applied to the text."), GUILayout.Width (130));
												_compass.textScale = EditorGUILayout.Slider (_compass.textScale, 0.02f, 3);
												EditorGUILayout.EndHorizontal ();
			
												EditorGUILayout.BeginHorizontal ();
												GUILayout.Label (new GUIContent ("Reveal Duration", "Text reveal duration in seconds."), GUILayout.Width (130));
												_compass.textRevealDuration = EditorGUILayout.Slider (_compass.textRevealDuration, 0, 3);
												EditorGUILayout.EndHorizontal ();
			
												EditorGUILayout.BeginHorizontal ();
												GUILayout.Label (new GUIContent ("Letter Delay", "Delay in appearance of each letter during a text reveal."), GUILayout.Width (130));
												_compass.textRevealLetterDelay = EditorGUILayout.Slider (_compass.textRevealLetterDelay, 0, 1);
												EditorGUILayout.EndHorizontal ();
			
												EditorGUILayout.BeginHorizontal ();
												GUILayout.Label (new GUIContent ("Duration", "Text duration in screen."), GUILayout.Width (130));
												_compass.textDuration = EditorGUILayout.Slider (_compass.textDuration, 0, 20);
												EditorGUILayout.EndHorizontal ();
			
												EditorGUILayout.BeginHorizontal ();
												GUILayout.Label (new GUIContent ("Fade Out Duration", "Duration of the text fade out."), GUILayout.Width (130));
												_compass.textFadeOutDuration = EditorGUILayout.Slider (_compass.textFadeOutDuration, 0, 10);
												EditorGUILayout.EndHorizontal ();
			
												EditorGUILayout.EndVertical ();

												EditorGUILayout.Separator ();
			

												if (_compass.isDirty) {
																EditorUtility.SetDirty (target);
												}
								}

								Texture2D MakeTex (int width, int height, Color col) {
												Color[] pix = new Color[width * height];
			
												for (int i = 0; i < pix.Length; i++)
																pix [i] = col;
			
												TextureFormat tf = SystemInfo.SupportsTextureFormat (TextureFormat.RGBAFloat) ? TextureFormat.RGBAFloat : TextureFormat.RGBA32;
												Texture2D result = new Texture2D (width, height, tf, false);
												result.SetPixels (pix);
												result.Apply ();
			
												return result;
								}

								GUIStyle titleLabelStyle;

								void DrawTitleLabel (string s) {
												if (titleLabelStyle == null) {
																titleLabelStyle = new GUIStyle (GUI.skin.label);
												}
												titleLabelStyle.normal.textColor = titleColor;
												titleLabelStyle.fontStyle = FontStyle.Bold;
												GUILayout.Label (s, titleLabelStyle);
								}


				}

}
