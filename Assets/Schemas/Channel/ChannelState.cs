// 
// THIS FILE HAS BEEN GENERATED AUTOMATICALLY
// DO NOT CHANGE IT MANUALLY UNLESS YOU KNOW WHAT YOU'RE DOING
// 
// GENERATED USING @colyseus/schema 0.5.32
// 

using Colyseus.Schema;

public class ChannelState : Schema {
	[Type(0, "map", typeof(MapSchema<ChannelPlayer>))]
	public MapSchema<ChannelPlayer> players = new MapSchema<ChannelPlayer>();
}

