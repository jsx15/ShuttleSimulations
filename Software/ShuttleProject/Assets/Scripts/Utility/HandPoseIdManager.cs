namespace Scripts
{
    public class HandPoseIdManager
    {
        /*
         * Current IDs
         */
        private string _currentHandIdLeft, _currentHandIdRight;

        /*
         * Getter / Setter
         */
        public string CurrentHandIdLeft
        {
            get => _currentHandIdLeft;
            set => _currentHandIdLeft = value;
        }

        public string CurrentHandIdRight
        {
            get => _currentHandIdRight;
            set => _currentHandIdRight = value;
        }
    }
}