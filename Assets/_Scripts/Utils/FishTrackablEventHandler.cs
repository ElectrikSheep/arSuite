/*==============================================================================
Copyright (c) 2010-2014 Qualcomm Connected Experiences, Inc.
All Rights Reserved.
Confidential and Proprietary - Qualcomm Connected Experiences, Inc.
==============================================================================*/

using UnityEngine;
using System ;

namespace Vuforia
{
	/// <summary>
	/// A custom handler that implements the ITrackableEventHandler interface.
	/// </summary>
	public class FishTrackablEventHandler : MonoBehaviour,
	ITrackableEventHandler
	{
		#region PRIVATE_MEMBER_VARIABLES
		
		private TrackableBehaviour mTrackableBehaviour;
		
		#endregion // PRIVATE_MEMBER_VARIABLES
		
		
		public static Action gainedTracking ;
		public static Action lostTracking ;

		
		#region UNTIY_MONOBEHAVIOUR_METHODS
		
		void Start()
		{
			mTrackableBehaviour = GetComponent<TrackableBehaviour>();
			if (mTrackableBehaviour)
			{
				mTrackableBehaviour.RegisterTrackableEventHandler(this);
			}
		}
		
		#endregion // UNTIY_MONOBEHAVIOUR_METHODS
		
		
		
		#region PUBLIC_METHODS
		
		/// <summary>
		/// Implementation of the ITrackableEventHandler function called when the
		/// tracking state changes.
		/// </summary>
		public void OnTrackableStateChanged(
			TrackableBehaviour.Status previousStatus,
			TrackableBehaviour.Status newStatus)
		{

			if (newStatus == TrackableBehaviour.Status.DETECTED ||
			    newStatus == TrackableBehaviour.Status.TRACKED ||
			    newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
			{
				OnTrackingFound();
				return ;
			}

			if (newStatus == TrackableBehaviour.Status.UNKNOWN) {
				return ;
			}
			if (previousStatus == TrackableBehaviour.Status.UNKNOWN) {
				return ;
			}

			OnTrackingLost ();
		}
		
		#endregion // PUBLIC_METHODS
		
		
		
		#region PRIVATE_METHODS
		
		
		private void OnTrackingFound()
		{
			Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " found");
			if( gainedTracking != null ) gainedTracking() ;
		}
		
		
		private void OnTrackingLost()
		{
			Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " lost");
			if( lostTracking != null ) lostTracking() ;
		}
		
		#endregion // PRIVATE_METHODS
	}
}

