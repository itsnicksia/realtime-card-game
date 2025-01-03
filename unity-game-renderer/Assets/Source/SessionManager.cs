using System;
using System.Linq;
using UnityEngine;
using Unity.Services.Core;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Multiplayer;

public class SessionManager : MonoBehaviour
{
	public string defaultSessionId = "devroom";

	// Debug
	public string statusString = "not connected";
	public ISession Session;
	private float _timeUntilNextRetry = 5f;

	public

	async void Start()
    {
	    await SignIn();

		#if UNITY_WEBGL && !DEBUG
			Session = await CreateOrJoinSession(defaultSessionId);
	    #else
			var joinedSessionIds = await MultiplayerService.Instance.GetJoinedSessionIdsAsync();
			var firstJoinedSessionId = joinedSessionIds.FirstOrDefault();
			Session = firstJoinedSessionId != null
			    ? await RejoinSession(firstJoinedSessionId)
				: await CreateOrJoinSession(defaultSessionId);
	    #endif
    }

	async void Update()
	{
		_timeUntilNextRetry -= Time.deltaTime;
		#if UNITY_WEBGL && !DEBUG
		Session = MultiplayerService.Instance.Sessions.FirstOrDefault().Value;
		if (Session == null && _timeUntilNextRetry <= 0f)
		{
			statusString = $"Retrying Session Join...";
			//CreateOrJoinSession(defaultSessionId);
			RejoinSession(defaultSessionId);

		}
		#endif
		_timeUntilNextRetry = 5f;
	}

	private async Task<ISession> RejoinSession(string sessionId)
	{
		statusString = $"Rejoining Session {sessionId}...";
		var session = await MultiplayerService.Instance.ReconnectToSessionAsync(sessionId);

		#if UNITY_WEBGL
			Session = session;
		#endif


		statusString =  $"Session {session.Id} rejoined! Join code: {session.Code}";
		return session;
	}

	private async Task SignIn()
    {
	    await UnityServices.InitializeAsync();
	    await AuthenticationService.Instance.SignInAnonymouslyAsync();
	    statusString =  $"Sign in anonymously succeeded! PlayerID: {AuthenticationService.Instance.PlayerId}";
    }

    private async Task<ISession> CreateOrJoinSession(string sessionId)
    {
	    statusString =  $"Creating Session {sessionId}...";
        var options = new SessionOptions
        {
    	    MaxPlayers = 6
        }.WithRelayNetwork();

        try
        {
	        var session = await MultiplayerService.Instance.CreateOrJoinSessionAsync(sessionId, options);
	        statusString = $"Joined session {session.Id}! Join code: {session.Code}";
			#if UNITY_WEBGL
				Session = session;
			#endif
	        return session;
        }
        catch (SessionException e)
        {
	        statusString = e.Error.ToString();
	        return await RejoinSession(sessionId);
        }
    }

    public void NotifyConnectedHack()
    {
	    Session = MultiplayerService.Instance.Sessions.FirstOrDefault().Value;
	    statusString = $"Joined session {Session.Id}! Join code: {Session.Code}";
    }
}
