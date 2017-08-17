namespace CombatRadar
{
    using Android.Widget;

    public class Status
    {
        private readonly TextView _statusTextControl;

        public Status(TextView statusTextControl)
        {
            _statusTextControl = statusTextControl;
        }

        public void SetStatus(string status)
        {
            _statusTextControl.Text = status;
        }
    }
}