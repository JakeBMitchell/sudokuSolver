using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sudokuSolverFixed
{

    //Class file to solve inputted sudoku
    public class sudokuSolver
    {

        //variable to store the sudoku board
        int[,] board;

        //constructed to pass inputted board into class
        public sudokuSolver(int[,] passed)
        {
            board = passed;
        }

        //function to check a 3x3 "box" if a value can be placed in that box
        public bool checkBox(int[,] board, int box, int val)
        {
            //calculates top left corner of "box" based off values labled below
            int sRow = (box / 3) * 3;
            int sCol = (box % 3) * 3;
            //loops through row/col
            for (int col = 0; col < 3; col++)
            {
                //loops through row/col
                for (int row = 0; row < 3; row++)
                {
                    //checks to see if value is present
                    if (board[row + sRow, col + sCol] == val || val > 9)
                    {
                        //if value is not present, return false
                        return false;
                    }
                }
            }
            //if value is present return true
            return true;
        }

        //function to check is value can be inserted in a specific row
        public bool checkRow(int[,] board, int row, int val)
        {
            //loop through row
            for (int col = 0; col < 9; col++)
            {
                //check if value is present
                if (board[row, col] == val)
                {
                    //return false if value is not present
                    return false;
                }
            }
            //return true if value is present
            return true;
        }


        //function to check if value can be inserted into specific column
        public bool checkcolumn(int[,] board, int col, int val)
        {
            //loop through the column
            for (int row = 0; row < 9; row++)
            {
                //if the value is not present return false
                if (board[row, col] == val)
                {
                    return false;
                }
            }
            //if the value is present return true
            return true;
        }

        //function that combines the three sepereate checkBox, col, and row functions
        public bool checkFunctions(int[,] board, int row, int col, int val)
        {
            //mathmatic function to find the box from row and column value, box labeled below
            int box = ((row / 3) * 3) + (col / 3);
            //checks if returns true
            if (checkBox(board, box, val) && checkcolumn(board, col, val) && checkRow(board, row, val) == true)
            {
                //value can be placed
                return true;
            }
            //otherwise value cannot be placed
            return false;
        }

        //function to check if the board is already completed
        public bool boardCompleted(int[,] board)
        {
            //loop through col/row
            for (int col = 0; col < 9; col++)
            {
                //loop through col/row
                for (int row = 0; row < 9; row++)
                {
                    //if no 0's found, board completed
                    if (board[row, col] == 0)
                    {
                        return false;
                    }
                }
            }
            //otherwise not completed
            return true;
        }

        //function to check each location and place possible values
        public bool completeBoard(int[,] board)
        {
            //if board already complete, return true
            if (boardCompleted(board) == true)
            {
                return true;
            }

            //values for heurisitc assignment to increase efficiency
            //spot with the fewest possible values
            int currentMin = 10;
            //store the positional data for that specific spot
            int posOfRow = 0;
            int posOfCol = 0;

            //loop through board
            for (int col = 0; col < 9; col++)
            {
                for (int row = 0; row < 9; row++)
                {
                    //counter for the number of possible answers
                    int counter = 0;
                    //if the spot is not filled, aka 0
                    if (board[row, col] == 0)
                    {
                        //calculate the number of possible solutions and increment counter
                        for (int count = 1; count <= 9; count++)
                        {
                            if (checkFunctions(board, row, col, count) == true)
                            {
                                counter++;
                            }
                        }
                        //set new spot with the fewest possible values and store position
                        if (counter < currentMin)
                        {
                            currentMin = counter;
                            posOfRow = row;
                            posOfCol = col;
                        }
                    }
                }
            }
            //loop for all of the possible answers, 1 - 9
            for (int count = 1; count <= 9; count++)
            {
                //with positional data, begin filling in answers at the spot with fewest possible answers
                if (checkFunctions(board, posOfRow, posOfCol, count) == true)
                {
                    //fill spot
                    board[posOfRow, posOfCol] = count;
                    //if the board is complete, return true
                    if (completeBoard(board) == true)
                    {
                        return true;
                    }
                    //backtracking for if spot cannot be filled
                    board[posOfRow, posOfCol] = 0;
                }
            }
            //otherwise, return false as the board was no completed
            return false;
        }

        //print function to display board
        public void printBoard(int[,] board)
        {
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    Console.Write(board[row, col] + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            //sudoku board found online, with predefined "squares (0-8)" labled for mathmatic purposes
            int[,] game = new int[9, 9] { 
                //   0         1         2
                { 8, 3, 1,  6, 0, 0,  0, 0, 7 },
                { 0, 7, 0,  0, 0, 0,  0, 0, 0 },
                { 0, 0, 0,  0, 2, 0,  0, 0, 0 }, 

                //   3         4         5
                { 0, 0, 0,  0, 0, 0,  8, 0, 0 },
                { 0, 0, 0,  2, 0, 0,  0, 0, 5 },
                { 0, 8, 0,  7, 0, 0,  0, 9, 6 }, 

                //   6         7         8
                { 0, 0, 0,  1, 0, 5,  4, 0, 0 },
                { 4, 0, 0,  0, 0, 0,  0, 7, 0 },
                { 9, 0, 0,  0, 0, 4,  0, 6, 1 }, };

            //object of sudokuSolver class
            sudokuSolver board1 = new sudokuSolver(game);
            //function tests
            Console.WriteLine("Box function check: Box 0, Value 9\nFalse: Contains value, True: Does not contain value");
            Console.WriteLine(board1.checkBox(game, 0, 9) + "\n\n");
            Console.WriteLine("Row function check: Row 0, Value 8\nFalse: Contains value, True: Does not contain value");
            Console.WriteLine(board1.checkRow(game, 0, 8) + "\n\n");
            Console.WriteLine("Col function check: Col 5, Value 4\nFalse: Contains value, True: Does not contain value");
            Console.WriteLine(board1.checkcolumn(game, 5, 4) + "\n\n");
            Console.WriteLine("Check to see if value can be placed at specific spot: Value 9");
            Console.WriteLine(board1.checkFunctions(game, 8, 3, 9) + "\n\n");
            //fill in the board
            board1.completeBoard(game);
            //print the board
            board1.printBoard(game);

        }
    }
}
