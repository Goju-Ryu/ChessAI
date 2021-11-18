using ChessAI.DataClasses;

namespace ChessAI.MoveSelection.StateAnalysis
{
    public interface IStateAnalyser
    {
        /**
         * <summary>A method representing some logic to calculate the value of the state</summary>
         * <param name="state">The state that should be Analyses</param>
         * <param name="isWhite">Should the analysis be done from the perspective of white or black</param>
         * <returns>
         * A numeric value that represents how good a state is. A positive value is in favor of this engine
         * while a negative one is in favor of its opponent no matter their color
         * </returns>
         */
        int StaticAnalysis(GameState state, bool isWhite);
    }
}