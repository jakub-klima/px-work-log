namespace PxWorkLog.Core
{
    public class WorkLogCore
    {
        public IssueCollection Issues { get; } = new IssueCollection();
        public IssueLogger Logger { get; }
        public BackupPath BackupPath { get; } = new BackupPath();

        public WorkLogCore()
        {
            Logger = new IssueLogger(Issues);
            Logger.StartStop(Issues[0]);
        }
    }
}
