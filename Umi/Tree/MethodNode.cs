using System;

namespace Umi.Tree{
	public class MethodNode : Node{
		private Node node;
		private Node x;
		private string message;
		public MethodNode(Node node,Node x,string message){
			this.node = node;
			this.x = x;
			this.message = message;
		}
		public override Node eval ()
		{
			return send_message(node.eval (),x.eval (),message);
		}
	}
}

