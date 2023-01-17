using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;


namespace NodeCanvas.Tasks.Conditions{

	public class IsPlayerInRange : ConditionTask
	{

		[RequiredField]
		public BBParameter<GameObject> player;
		public BBParameter<GameObject> self;
		public BBParameter<float> attentionRange;

		//Use for initialization. This is called only once in the lifetime of the task.
		//Return null if init was successfull. Return an error string otherwise
		protected override string OnInit(){
			return null;
		}

		//Called whenever the condition gets enabled.
		protected override void OnEnable(){
			
		}

		//Called whenever the condition gets disabled.
		protected override void OnDisable(){
			
		}

		//Called once per frame while the condition is active.
		//Return whether the condition is success or failure.
		protected override bool OnCheck()
		{
			if (player == null || self == null || attentionRange == null)
			{
				Debug.LogError("please assign player || self");
			}

			Vector3 distance3D = player.value.gameObject.transform.position - self.value.gameObject.transform.position;
			float distance = Mathf.Sqrt(distance3D.x * distance3D.x + distance3D.y * distance3D.y);
			if (distance > attentionRange.value)
			{
				return false;
			}
			else
			{
				return true;
			}
		}
	}
}