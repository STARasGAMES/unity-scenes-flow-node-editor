using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using XNode;

namespace SceneFlowNodeEditor
{
    [CreateAssetMenu]
    public class ScenesFlowNodeGraph : NodeGraph
    {

        public List<string> GetLinksFromScene(string scenePath)
        {
            var sceneNode = nodes.OfType<SceneNode>().FirstOrDefault(x => x.sceneReference.ScenePath == scenePath);
            if (sceneNode == null)
                throw new Exception($"Graph doesn't contain SceneNode with scene: '{scenePath}'");
            var port = sceneNode.GetPort("Links");
            var connection = port.GetConnections();
            var result = connection.Select(x => (x.node as SceneNode).sceneReference.ScenePath).ToList();
            return result;
        } 
        
        #if UNITY_EDITOR
        private void OnValidate()
        {
            var scenes = EditorBuildSettings.scenes;
            var nodes = this.nodes.OfType<SceneNode>().ToArray();
            foreach (var scene in scenes)
            {
                var scenePath = scene.path;
                int count = nodes.Count(x => x.sceneReference.ScenePath == scenePath);
                if (count == 0)
                {
                    var position = GetPositionForNewNode();
                    var newNode = AddNode<SceneNode>();
                    newNode.position = position;
                    newNode.sceneReference = new SceneReference();
                    newNode.sceneReference.ScenePath = scenePath;
                }
                else if (count > 1)
                {
                    Debug.LogError($"Several nodes with the same sceneReference reference: '{scenePath}'");
                }
            }
        }

        private Vector2 GetPositionForNewNode()
        {
            var lastNode = nodes.FirstOrDefault();
            if (lastNode == null)
                return Vector2.zero;
            
            for (int i = 1; i < nodes.Count; i++)
            {
                if (lastNode.position.x < nodes[i].position.x)
                    lastNode = nodes[i];
            }

            return lastNode.position + Vector2.right * 300;
        }
        #endif
    }
}