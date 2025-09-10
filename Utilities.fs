module Utilities

let mapTriple (f: 'a -> 'b) (triple: 'a * 'a * 'a) : 'b * 'b * 'b =
    match triple with
    | x, y, z -> (f x, f y, f z)
