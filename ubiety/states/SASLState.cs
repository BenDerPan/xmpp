// SASLState.cs
//
//Ubiety XMPP Library Copyright (C) 2006 - 2011 Dieter Lunn
//
//This library is free software; you can redistribute it and/or modify it under
//the terms of the GNU Lesser General Public License as published by the Free
//Software Foundation; either version 3 of the License, or (at your option)
//any later version.
//
//This library is distributed in the hope that it will be useful, but WITHOUT
//ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS
//FOR A PARTICULAR PURPOSE. See the GNU Lesser General Public License for more
//
//You should have received a copy of the GNU Lesser General Public License along
//with this library; if not, write to the Free Software Foundation, Inc., 59
//Temple Place, Suite 330, Boston, MA 02111-1307 USA

using ubiety.common;
using ubiety.logging;

namespace ubiety.states
{
	/// <summary>
	/// SASL state is used to authenticate the user with the current processor.
	/// </summary>
	public class SASLState : State
	{
		/// <summary>
		/// Execute the actions to authenticate the user.
		/// </summary>
		/// <param name="data">
		/// The <see cref="Tag"/> we received from the server.  Probably a challenge or response.
		/// </param>
		public override void Execute(Tag data)
		{
			Logger.Debug(this, "Processing next SASL step");
			var res = Current.Processor.Step(data);
			switch (res.Name)
			{
				case "success":
					Logger.Debug(this, "Sending start stream again");
					Current.Authenticated = true;
					Current.State = new ConnectedState();
					Current.Execute();
					break;
				case "failure":
					return;
				default:
					Current.Socket.Write(res);
					break;
			}
		}
	}
}