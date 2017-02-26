using System;
using System.Collections.Generic;

namespace Stamina.Utils {
	struct ActionPair {
		public Action Action;
		public int RunFor;
	}


	public class CustomHooks {
		private LinkedList<ActionPair> Hooks = new LinkedList<ActionPair>();
		
		public void AddHook( Action action, int times_run ) {
			if( times_run == 0 ) { return; }
			this.Hooks.AddLast( new ActionPair { Action = action, RunFor = times_run } );
		}

		public void RunHooks() {
			for( var node = this.Hooks.First; node != null; node = node.Next ) {
				var kv = node.Value;
				if( kv.RunFor == 1 ) {
					this.Hooks.Remove( node );
				} else if( kv.RunFor > 1 ) {
					kv.RunFor--;
				}
				kv.Action();
			}
		}
	}
}

//foreach( Action action in this.Hooks.Keys.ToArray() ) {
//	if( this.Hooks[action] == 1 ) {
//		this.Hooks.Remove( action );
//	} else if( this.Hooks[action] > 1 ) {
//		this.Hooks[action]--;
//	}
//	action();
//}
