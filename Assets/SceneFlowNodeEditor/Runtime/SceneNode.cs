using System;
using System.Collections.Generic;
using System.Linq;
using XNode;

namespace SceneFlowNodeEditor
{
    [Serializable]
    public struct Connection
    {
    }

    public class SceneNode : Node
    {
        [Input]
        public Connection input;

        public SceneReference sceneReference;

        [Output(dynamicPortList = true, connectionType = ConnectionType.Override)]
        public List<string> Links = new List<string>(){"default"};
    }
}