open System
open Errors
open Utilities
open Game

let rec getValidInput (gameState: GameState) : Board =
    Console.Write "Where do you want to place your mark (tl, tm, tr, ml, mm, mr, bl, bm, br)? "
    let input = Console.ReadLine()

    let newBoardResult =
        result {
            let! pos = tryReadPosition input
            let! newBoard = tryPlaceMark gameState.CurrentBoard gameState.CurrentPlayer pos

            return newBoard
        }

    match newBoardResult with
    | Ok newBoard -> newBoard
    | Error err ->
        Console.WriteLine(showPositionError err)
        getValidInput gameState

let rec gameLoop (gameState: GameState) : Outcome =
    Console.WriteLine(showGameState gameState)
    let newBoard = getValidInput gameState

    match tryGetOutcome newBoard with
    | None ->
        gameLoop
            { CurrentBoard = newBoard
              CurrentPlayer = otherPlayer gameState.CurrentPlayer }
    | Some outcome -> outcome

let showGameOutcome (outcome: Outcome) : string =
    match outcome with
    | Win(player, board) -> $"\n{showBoard board}\n\n🎉 Player {player} wins!"
    | Draw board -> $"\n{showBoard board}\n\n🤝 It's a draw."

let main =
    Console.WriteLine "Welcome to Tic-tac-toe"
    let outcome = gameLoop initialState
    Console.WriteLine(showGameOutcome outcome)
