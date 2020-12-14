namespace Scripts
{
    public class CarryIDManager
    {
        /*
         * Current IDs
         */
        private string _currentCarryIdLeft, _currentCarryIdRight, _currentCarryIdBoth;

        /*
         * Getter / Setter
         */
        public string CurrentCarryIdLeft
        {
            get => _currentCarryIdLeft;
            set => _currentCarryIdLeft = value;
        }

        public string CurrentCarryIdRight
        {
            get => _currentCarryIdRight;
            set => _currentCarryIdRight = value;
        }

        public string CurrentCarryIdBoth
        {
            get => _currentCarryIdBoth;
            set => _currentCarryIdBoth = value;
        }
        
    }
}