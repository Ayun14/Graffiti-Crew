using UnityEngine;

public class BoomerangNode : Node, INodeAction
{
    private BoomerangNodeDataSO _boomerangNodeData;
    private SpriteRenderer _renderer;

    // Sound
    private SoundObject _sprayLongSoundObj;

    private void Awake()
    {
        
    }

    public void NodeStartAction()
    {

    }

    public override NodeDataSO GetNodeDataSO() => _boomerangNodeData;

    public override NodeType GetNodeType() => _boomerangNodeData.nodeType;
}
