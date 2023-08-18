using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MainProject
{
    internal class Program
    {
        static bool terminate = false;

        static int user1Score = 0;
        static int user2Score = 0;
        static int personScore = 0;
        static int botScore = 0;
        static void Main(string[] args)
        {
            while (terminate == false)
            {
                MainWindow();
                // Console.WriteLine(UserInput.GetSpecific("정수내놔 : ", ""));
            }
        }

        static void MainWindow()
        {
            Console.WriteLine("WELCOME TO THE <<TICK TAE TO>> GAME !! ");
            Console.WriteLine("1. Play with your friend");
            Console.WriteLine("2. Play with the bot");
            Console.WriteLine("3. ScoreBoard");
            Console.WriteLine("4. exit");

            var mainMenuConnectionList = new List<Action> {PlayWithYourFriend, PlayWithBot, ScoreBoard, WindowExit};

            int intMenu = Int32.Parse(UserInput.GetSpecific("select the menu that you want to enter  (1 ~ 5) : ", "123", 1));
            intMenu += -1;

            mainMenuConnectionList[intMenu]();

        }

        static void PlayWithYourFriend()
        {
            var Board = new GameBoard3x3();

            Console.WriteLine("Welcome to the TickTaeTo Game.");
            Console.WriteLine("Current Game mode : User1 vs User2");

            bool isTurnUser1 = true;
            while (true)
            {
                // intruduce the game state
                if (isTurnUser1) {Console.WriteLine("USER1 (ⵔ) turn.");} else {Console.WriteLine("USER2 (✘) turn.");}
                Console.WriteLine(Board.PrintBoard());
                var userInputNum = UserInput.GetSpecific("Please put the number 1~9 you wan to place your marker  :",
                                                        "123456789",1);

                // proceed the turn
                Board.Next(Int32.Parse(userInputNum));

                // check if game has been over.
                var state = Board.BoardStateCheck();
                if (state != null) 
                {
                    Console.WriteLine(Board.PrintBoard());
                    if (state == 1) { Console.WriteLine("USER1 (ⵔ) WINs !!!"); user1Score++;}
                    else if(state == -1) { Console.WriteLine("USER2 (✘) WINs !!!"); user2Score++;}
                    else { Console.WriteLine("the TIE !!"); }
                    break;
                }
                if (isTurnUser1) {isTurnUser1 = false;} else { isTurnUser1 = true ;}
            }
            
            UserInput.Get("press enter to go back ...");
        }

        static void PlayWithBot()
        {
            var Board = new GameBoard3x3();

            Console.WriteLine("Welcome to the TickTaeTo Game.");
            Console.WriteLine("Current Game mode : Human vs Bot");
            Console.WriteLine();

            string userSymbol = UserInput.GetSpecific("Do you want to play First (ⵔ) or play Next (✘) ?   (o/x): ","ox",1);
            bool isFirstTurnHuman = String.Equals(userSymbol , "o") ? true : false; 
            bool isTurnFirst = true;


            while (true)
            {
                // intruduce the game state
                bool isCurrentTurnHuman = !(isFirstTurnHuman ^ isTurnFirst);
                if (isCurrentTurnHuman) {Console.Write("User ");} else {Console.Write("Bot ");}
                if (isTurnFirst) {Console.WriteLine("(ⵔ) turn.");} else {Console.WriteLine("(✘) turn.");}
                Console.WriteLine(Board.PrintBoard());

                if (isCurrentTurnHuman)
                {
                    var userInputNum = UserInput.GetSpecific("Please put the number 1~9 you wan to place your marker  :",
                                                            "123456789",1);

                    // proceed the turn
                    Board.Next(Int32.Parse(userInputNum));
                }
                else
                {
                    Board.NextAuto();
                }

                // check if game has been over.
                var state = Board.BoardStateCheck();
                if (state != null) 
                {
                    Console.WriteLine(Board.PrintBoard());
                    if (state == 1) 
                    { 
                        if (isFirstTurnHuman) {Console.WriteLine("Human (ⵔ) WINs !!!"); personScore++;}
                        else {Console.WriteLine("Bot (ⵔ) WINs !!!"); botScore++;}
                    }
                    else if(state == -1) 
                    { 
                        if (isFirstTurnHuman) {Console.WriteLine("Bot (✘) WINs !!!"); botScore++;}
                        else {Console.WriteLine("Human (✘) WINs !!!"); personScore++;}
                    }
                    else { Console.WriteLine("the TIE !!"); }
                    break;
                }
                if (isTurnFirst) {isTurnFirst = false;} else { isTurnFirst = true ;}
            }
            
            UserInput.Get("press enter to go back ...");
        }
        static void ScoreBoard()
        {
            Console.WriteLine("SCOREBOARD");
            Console.WriteLine("1. User1 VS User2 mode");
            if (user1Score >= user2Score) 
            {
                Console.WriteLine($"    USER 1 : {user1Score} point");
                Console.WriteLine($"    USER 2 : {user2Score} point");
            }
            else
            {
                Console.WriteLine($"    USER 2 : {user2Score} point");
                Console.WriteLine($"    USER 1 : {user1Score} point");
            }

            Console.WriteLine("2. Bot VS User mode");
            if (botScore >= personScore) 
            {
                Console.WriteLine($"    BOT : {botScore} point");
                Console.WriteLine($"    HUMAN : {personScore} point");
            }
            else
            {
                Console.WriteLine($"    HUMAN : {personScore} point");
                Console.WriteLine($"    BOT : {botScore} point");
            }
            UserInput.Get("press enter to go back ...  (any):");
        }
        
        
        static void WindowExit()
        {
            terminate = true;
            UserInput.Get("press enter to exit ...  (any):");
        }

    }

    public class GameBoard3x3
    {
        // board value will be the one of 0, -1, 1.
        // the first player's mark will be 1, and the next player's mark will be -1, and 0 is the empty space.
        // indexing of board is x-axis first priority, and right-x-down-y increasing order. ex) board[0][1] = -1 is :
        // 0   0   0
        // -1  0   0
        // 0   0   0
        // for user input, program will use 1~9 numbering, like
        // 1   2   3
        // 4   5   6
        // 7   8   9
        // plus, the FIRST PLAYER = O , the NEXT PLAYER = X, respectively.
        private List<List<short>> board = new List<List<short>>();

        private short currentTurn = 1; // 1 -> first player's turn will be preceed in next move, -1 then opposite.
        private bool state = true; // true -> board is live. false -> game is over. board is inactivated.
        private short winner = 0; // 0 : tie. 1 : first player wins. -1 : next player wins. have no meanings if state = true
        public GameBoard3x3()
        {
            board.Add(new List<short> {0,0,0});
            board.Add(new List<short> {0,0,0});
            board.Add(new List<short> {0,0,0});
        }

        private int[] UserNumToDimention(int userNum)
        {
            int xIndex = (userNum - 1) % 3;
            int yIndex = (userNum - 1) / 3;

            return new int[] {xIndex, yIndex};
        }

        /// <summary>
        /// winnerCheck
        /// decide winner.
        /// </summary>
        /// <returns>
        /// return 1 || -1 : winner decided.
        /// return 0 : winner not decided.
        /// </returns>
        private short winnerCheck()
        {
            // check the vertical lines.
            foreach(var line in board) 
            {
                if (line[0] != 0 && line[0] == line[1] && line[1] == line[2])
                {
                    return line[0]; // winner decided.
                }
            } 

            // check the horizontal lines.
            for(int yIndex = 0; yIndex < 3; yIndex++)
            {
                short equalCount = 0;
                short prevMark = board[0][yIndex];
                for(int xIndex = 1; xIndex<3; xIndex++)
                {
                    short targetMark = board[xIndex][yIndex];
                    if (prevMark == targetMark)
                    {
                        equalCount++;
                    }
                    prevMark = targetMark;
                }

                if (prevMark != 0 && equalCount == 2)
                {
                    return prevMark;
                }
            }

            //check the diagonals.
            if (board[0][0] != 0 && board[0][0] == board[1][1] && board[1][1] == board[2][2]) {return board[0][0];}
            if (board[0][2] != 0 && board[0][2] == board[1][1] && board[1][1] == board[2][0]) {return board[0][2];}
            
            return 0;
        }
        /// <summary>
        /// BoardFullCheck
        /// check if board is full, so the game must be end.
        /// </summary>
        /// <returns>
        /// return false : game can be keep going
        /// return true : game must be end. the board is full.
        /// </returns>
        private bool BoardFullCheck()
        {
            foreach (var lineList in board)
            {
                foreach (var mark in lineList)
                {
                    if (mark == 0)
                    {
                        return false;
                    } 
                }
            }

            return true;
        }

        /// <summary>
        /// Next
        /// play the turn of the current "currentTurn" player.
        /// </summary>
        /// <param name="num"></param>
        /// <returns>
        /// return 0 : successfully worked.
        /// return 1 : user number parameter missed from range.
        /// return 2 : state = false, method inactivated.
        /// </returns>
        public int Next(int num)
        {
            if (state == false) { return 2; }
            if (num <= 0 || 10 <= num) { return 1;}
            
            // mark the board.
            int[] indexBoard = UserNumToDimention(num);
            board[indexBoard[0]][indexBoard[1]] = currentTurn;

            // check if winner has decided.
            short winner = winnerCheck();
            if (winner != 0) 
            {
                state = false;
                this.winner = winner;
                return 0;
            }

            // check if the board is full
            if (BoardFullCheck())
            {
                state = false;
                this.winner = 0;
                return 0;

            }

            // pass the turn.
            if (currentTurn == 1) {currentTurn = -1;} else {currentTurn = 1;}
            return 0;
        }

        /// <summary>
        /// private int AutoEvaluate()
        /// evaluate the board then returns the optimal next move.
        /// </summary>
        /// <returns>
        /// return 0 : successfully worked.
        /// return 1 : user number parameter missed from range.
        /// return 2 : state = false, method inactivated.
        /// </returns>
        private int[]? AutoEvaluate()
        {
            int[] nextMoveCandidate = new int[2];

            // 

            // make the investigate, initialize function for repeated code
            int markCount = 0;
            bool isEmptyOccured = false;
            int emptyCount = 0;
            void investigate_myMark(int xIndex, int yIndex)
            {
                var mark = board[xIndex][yIndex];
                if (currentTurn == mark) {markCount++;} 
                else if(mark == 0) {isEmptyOccured = true; nextMoveCandidate = new int[] {xIndex, yIndex};}
            }
            void initialize()
            {
                markCount = 0;
                isEmptyOccured = false;
            }

            // make the evaluate function for repeated code,
            // which will return true only if the statement is true.
            bool evaluate(Func<bool> statement, Action<int, int> investigate, Action initialize)
            {
                initialize();
                // // check the verital lines.
                for (int xIndex = 0; xIndex<3; xIndex++)
                {
                    for (int yIndex = 0; yIndex < 3; yIndex++)
                    {
                        investigate(xIndex, yIndex);
                    }
                    if (statement()) { return true; }
                    
                    initialize();
                }

                // // check the horizontal lines
                initialize();
                for (int yIndex = 0; yIndex<3; yIndex++)
                {
                    for (int xIndex = 0; xIndex < 3; xIndex++)
                    {
                        investigate(xIndex, yIndex);
                    }
                    if (statement()) { return true; }
                    
                    initialize();
                }

                // // check the diagonal lines
                initialize();
                for (int i=0; i < 3; i++)
                {
                    int xIndex = i;
                    int yIndex = i;
                    investigate(xIndex, yIndex);

                    if (statement()) { return true; }
                }
                initialize();
                for (int i=0; i < 3; i++)
                {
                    int xIndex = i;
                    int yIndex = 2-i;
                    investigate(xIndex, yIndex);

                    if (statement()) { return true; }
                }

                // no case catched -> return false.
                return false;
            }
            
            // evaluate priority rank 1 : the move that makes you win
            bool isNextMoveDecided = evaluate(() => markCount == 2 && isEmptyOccured == true, investigate_myMark, initialize);
            if (isNextMoveDecided) {return nextMoveCandidate;}

            // evaluate priority rank 2 : the move that prevent the opponent from winning
            void investigate_opponentMark(int xIndex, int yIndex)
            {
                var mark = board[xIndex][yIndex];
                var opponentTurn = (currentTurn == 1) ? -1 : 1;
                if (opponentTurn == mark) {markCount++;} 
                else if(mark == 0) {isEmptyOccured = true; nextMoveCandidate = new int[] {xIndex, yIndex};}
            }
            isNextMoveDecided = evaluate(() => markCount == 2 && isEmptyOccured == true, investigate_opponentMark, initialize);
            if (isNextMoveDecided) {return nextMoveCandidate;}

            // evaluate priority rank 3 : the center position
            if (board[1][1] == 0) { return new int[] {1,1} ;}

            // evaluate priority rank 4 : double-serize making
            void investigate_canDouble(int xIndex, int yIndex)
            {
                var mark = board[xIndex][yIndex];
                if (currentTurn == mark) {markCount++;} 
                else if(mark == 0) {nextMoveCandidate = new int[] {xIndex, yIndex}; emptyCount++;}
            }

            void initialize_canDouble()
            {
                markCount = 0;
                emptyCount = 0;
            }
            isNextMoveDecided = evaluate(() => markCount == 1 && emptyCount == 2, investigate_canDouble, initialize_canDouble);
            if (isNextMoveDecided) {return nextMoveCandidate;}

            // evaluate priority rank 5 : random haha
            for (int xIndex = 1; xIndex<3; xIndex++)
            {
                for (int yIndex = 0; yIndex < 3; yIndex++)
                {
                    if (board[xIndex][yIndex] == 0) {return new int[] {xIndex, yIndex};}
                }
            }
            return null;
        }

        /// <summary>
        /// public int NextAuto()
        /// play turn automatically without receiving the user input position num.
        /// </summary>
        /// <returns></returns>
        public int NextAuto()
        {
            if (state == false) { return 2; }

            // mark the board.
            int[] indexBoard = AutoEvaluate();
            board[indexBoard[0]][indexBoard[1]] = currentTurn;

            // check if winner has decided.
            short winner = winnerCheck();
            if (winner != 0) 
            {
                state = false;
                this.winner = winner;
                return 0;
            }

            // check if the board is full
            if (BoardFullCheck())
            {
                state = false;
                this.winner = 0;
                return 0;

            }

            // pass the turn.
            if (currentTurn == 1) {currentTurn = -1;} else {currentTurn = 1;}
            return 0;
        }

        /// <summary>
        /// public string PrintBoard()
        /// print the current board marks.
        /// returns the whole one string with \n.
        /// </summary>
        /// <returns></returns>
        public string PrintBoard()
        {
            var resultString = "";

            for (int yIndex = 0; yIndex < 3; yIndex++)
            {
                for (int xIndex = 0; xIndex < 3; xIndex++)
                {
                    var targetMark = board[xIndex][yIndex];
                    var stringMark = "";
                    if (targetMark == -1)
                    {
                        stringMark = "✘";
                    }
                    else if (targetMark == 1)
                    {
                        stringMark = "ⵔ";
                    }
                    else
                    {
                        stringMark = (xIndex + 1 + (yIndex * 3)).ToString();
                    }
                    resultString += stringMark + " | ";
                }
                resultString += "\n----------\n";
            }

            return resultString;

        }

        /// <summary>
        /// public int? BoardStateCheck()
        /// check the current board state. 
        /// case is divided like [not decided], [X wins], [O wins], [Tie].
        /// 
        /// WARNING : this function just check the private member this.state, this.winner,
        /// without directly checking the this.board.
        /// 
        /// </summary>
        /// <returns>
        /// return 0 : Tie.
        /// return 1 : O wins. (the first player)
        /// return -1 : X wins. (the next player)
        /// return null : not decided.
        /// </returns>
        public int? BoardStateCheck()
        {
            if ( state == true )
            {
                return null;
            }
            else
            {
                return winner;
            }
        }
    }
    public static class DebugConsole
    {
        public static void D1List(dynamic list)
        {
            try 
            {
                Console.Write("{");
                foreach (dynamic d in list)
                {
                    Console.Write(d.ToString());
                    Console.Write(',');
                }
                Console.Write('}');
            }
            catch (Exception e)
            {
                Console.Write("unexpected exception happens.");
                Console.Write(e);
            }

        }
    }


    public static class UserInput
    {
        /// <summary>
        /// GetUserInput
        /// print the description through console then get the user input string.
        /// </summary>
        /// <returns></returns>
        public static string Get(string description) 
        {
            while (true)
            {
                Console.Write(description);
                var result = Console.ReadLine();
                if (result == null)
                {
                    continue;
                }
                else
                {
                    return result;
                }
            }
        }
        /// <summary>
        /// GetSpecific
        /// do same as Get but only accept the value that only made of acceptableCollection string members
        /// if other string has found, then re-ask to user again.
        /// </summary>
        /// <param name="description">non-empty string.</param>
        /// <param name="acceptableCollection"></param>
        /// <returns></returns>
        public static string GetSpecific(string description, string acceptableCollection)
        {
            if (acceptableCollection.Length == 0)
            {
                throw new System.ArgumentException("param acceptableCollection must be non-empty string.");
            }
            string? userInput = "";
            while (true)
            {
                Console.Write(description);
                userInput = Console.ReadLine();
                if (userInput == null) {continue;}
                bool isAcceptable = true;
                foreach (var c in userInput) 
                {
                    if (!acceptableCollection.Contains(c))
                    {
                        isAcceptable = false;
                        break;
                    }
                }
                if (isAcceptable == false)
                {
                    continue;
                }
                else 
                {
                    return userInput;
                }
            }
        }
        /// <summary>
        /// GetSpecific(string description, string acceptableCollection, int length)
        /// input length specified version of UserInput.GetSpecific().
        /// </summary>
        /// <param name="description"></param>
        /// <param name="acceptableCollection">non-empty string</param>
        /// <param name="length">positive integer</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException"></exception>
        public static string GetSpecific(string description, string acceptableCollection, int length)
        {
            if (acceptableCollection.Length == 0)
            {
                throw new System.ArgumentException("param acceptableCollection must be non-empty string.");
            }
            if (length < 1)
            {
                throw new System.ArgumentException("param length must be positive integer.");
            }
            string? userInput = "";
            while (true)
            {
                Console.Write(description);
                userInput = Console.ReadLine();
                if (userInput == null || userInput.Length != length) {continue;}
                bool isAcceptable = true;
                foreach (var c in userInput) 
                {
                    if (!acceptableCollection.Contains(c))
                    {
                        isAcceptable = false;
                        break;
                    }
                }
                if (isAcceptable == false)
                {
                    continue;
                }
                else 
                {
                    return userInput;
                }
            }
        }

    }
}
