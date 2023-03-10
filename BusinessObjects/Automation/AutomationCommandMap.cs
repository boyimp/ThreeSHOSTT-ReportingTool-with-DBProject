using ThreeS.Infrastructure.Data;

namespace ThreeS.Domain.Models.Automation
{
    public class AutomationCommandMap : AbstractMap
    {
        public int AutomationCommandId { get; set; }
        public bool DisplayOnTicket { get; set; }
        public bool DisplayOnPayment { get; set; }
        public bool DisplayOnOrders { get; set; }
        public string EnabledStates { get; set; }
        public string VisibleStates { get; set; }

        public override void Initialize()
        {
            DisplayOnTicket = true;
            EnabledStates = "*";
            VisibleStates = "*";
            base.Initialize();
        }
    }
}
