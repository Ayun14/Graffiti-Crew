public interface BattleGameRule
{
    protected void Init(NodeSpawner spawner, NodeJudgement judgement);
    protected void Update();
    protected void OnNodeCleared();
}
