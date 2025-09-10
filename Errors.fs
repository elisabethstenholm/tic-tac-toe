module Errors

type InputError =
    | UnknownPosition of string
    
let showInputError (inputError: InputError) : string =
    match inputError with
    | UnknownPosition pos -> $"Unknown position: {pos}"

type PositionError =
    | PositionTaken
    
let showPositionError (positionError: PositionError) : string =
    match positionError with
    | PositionTaken -> "Position already taken!"
