namespace ChessAI.DataClasses
{
    /**
     * <summary>
     * dummy struct representing a game state in some way
     * </summary>
     */
    public readonly struct GameState
    {
        public GameState(string state)
        {
            State = state;
        }

        //Implementation goes here...
        //TODO replace string state with actual implementation
        public string State { get; }

        /**
         * <summary>A dummy method representing some logic to calculate the applying a move to the state</summary>
         * <param name="move">The move that should be applied to a state</param>
         * <returns>A new <see cref="GameState"/> with the move applied</returns>
         */
        public GameState ApplyMove(string move)
        {
            //implementation goes here...
            return new GameState(move);
        }
    }
}