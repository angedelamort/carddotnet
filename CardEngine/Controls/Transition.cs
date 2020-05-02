namespace CardEngine.Controls
{
    // todo: use an nuget for animation - stupid... in Xamarin or WinForm...
    // https://github.com/UweKeim/dot-net-transitions/tree/master/Transitions
    public class Transition
    {
        /// <summary>
        /// Transition loop
        /// </summary>
        /// <returns>False if the animation has ended.</returns>
        public bool Execute() // probably pass elapsedTime in here...
        {
            return false;
        }
    }
}
