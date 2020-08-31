using UnityEngine;
using System.Collections;

namespace CompassNavigatorPro
{
	public enum TITLE_VISIBILITY {
		OnlyWhenVisited = 0,
		Always = 1
	}

	public delegate void OnHeartbeatEvent();

	[AddComponentMenu("Compass Navigator Pro/Compass POI")]
	[ExecuteInEditMode]
	public class CompassProPOI : MonoBehaviour
	{
		[Tooltip("Title to be shown when this POI is in the center of the compass bar and it's a known location (isVisited = true)")]
		public string title;

		[Tooltip("Rule for title visibility.")]
		public TITLE_VISIBILITY titleVisibility = TITLE_VISIBILITY.OnlyWhenVisited;

		[Tooltip("Specifies if this POI is visible in the compass bar.")]
		public bool isVisible;

		[Tooltip("Specifies if POI must be removed from compass bar when visited.")]
		public bool hideWhenVisited;
		
		[Tooltip("Specifies if this POI can be marked as visited when reached.")]
		public bool canBeVisited = true;

		[Tooltip("Specifies if this POI has been already visited.")]
		public bool isVisited;

		[Tooltip("Text to show when discovered. Leave this to null if you don't want to show any text.")]
		public string visitedText;
		
		[Tooltip("Sound to play when POI is visited the first time.")]
		public AudioClip visitedAudioClip;

		[Tooltip("Radius of activity of this POI. Useful for area POIs.")]
		public float radius;

		[Tooltip("The icon for the POI if has not been discovered/visited.")]
		public Sprite iconNonVisited;

		[Tooltip("The icon for the POI if has been visited.")]
		public Sprite iconVisited;
		
		[Tooltip("If the icon will be shown in the scene during playmode. If enabled, the icon will fade in smoothly as the player approaches it.")]
		public bool showPlayModeGizmo;
		
		[Tooltip("If enabled, the icon will stop at the edges of the bar even if it's behind the player.")]
		public bool clampPosition;

		[Tooltip("Sound to play when beacon is shown.")]
		public AudioClip beaconAudioClip;
		
		[Tooltip("Preserves the state of this POI between scene changes. Note that this POI only will be visible in the scene where it was first created.")]
		public bool dontDestroyOnLoad;

		[Tooltip("Enables heartbeat effect. Plays a sound with variable speed when approaching this POI.")]
		public bool heartbeatEnabled;

		[Tooltip("Sound to play when heartbeat effect is enabled.")]
		public AudioClip heartbeatAudioClip;
		
		[Tooltip("Distance to start playing heartbeat effect is enabled.")]
		public float heartbeatDistance = 20f;
		
		[Tooltip("Interval of heartbeat rate based on distance.")]
		public AnimationCurve heartbeatInterval = AnimationCurve.Linear(0,0.25f,1f,3f);

		public OnHeartbeatEvent OnHeartbeat;

		[HideInInspector]
		public float distanceToCameraSQR;

		[HideInInspector]
		public Vector3 iconScale;

		[HideInInspector]
		public float iconAlpha;

		[HideInInspector]
		public SpriteRenderer spriteRenderer; // in-scene gizmo

		[HideInInspector]
		public int id; // Unique ID to be used when DontDestroyOnLoad is set to true. Otherwise it's not used.

		[HideInInspector]
		public float visibleTime; // time where the poi appeared on the compass bar

		[HideInInspector]
		public bool heartbeatIsActive;

		Coroutine heartbeatPlayer;

		void OnEnable() {
			if (id==0) {
				id = System.Guid.NewGuid().GetHashCode();
			}
		}

		// Use this for initialization
		void Start ()
		{
			CompassPro compass = CompassPro.instance;
			if (compass==null) return;

			if (dontDestroyOnLoad && Application.isPlaying) {
				if (compass.POIisRegistered(this)) {
					Destroy (gameObject);
					return;
				}
				DontDestroyOnLoad(gameObject);
			}

			compass.POIRegister (this);
		}

		void OnDrawGizmos() {
			Gizmos.DrawIcon(transform.position, "compassIcon.png", true);
		}


		public void StartHeartbeat() {
			if (isVisited) return;
			heartbeatPlayer = StartCoroutine(HeartBeatPlayer());
			heartbeatIsActive = true;
		}

		public void StopHeartbeat() {
			if (heartbeatPlayer!=null) StopCoroutine(heartbeatPlayer);
			heartbeatIsActive = false;
		}

		IEnumerator HeartBeatPlayer() {
			AudioClip heartbeatSound = heartbeatAudioClip != null ? heartbeatAudioClip : CompassPro.instance.heartbeatDefaultAudioClip;
			if (heartbeatSound==null) {
				Debug.LogWarning ("Compass POI: heartbeat sound not set.");
				yield break;
			}
			heartbeatDistance = Mathf.Max (1f, heartbeatDistance);
			float minDistance = CompassPro.instance.visitedDistance;
			while(true) {
				float distance = Vector3.Distance(Camera.main.transform.position, transform.position);
				if (distance>heartbeatDistance || isVisited) {
					heartbeatIsActive = false;
					yield break;
				}
				if (distance < minDistance) distance = minDistance;
				AudioSource.PlayClipAtPoint(heartbeatSound, Camera.main.transform.position);
				if (OnHeartbeat!=null) OnHeartbeat();
				float curvePos = (distance - minDistance) / heartbeatDistance;
				float delay = heartbeatInterval.Evaluate(curvePos);
				yield return new WaitForSeconds(delay);
			}
		}
	
	}

}