# Tic-tac-toe i F#

I dette repoet får du prøve på F# ved å lage det klassiske spillet Tic tac toe. Repoet inneholder skjelettet til spillet,
men noen funksjoner er feil. De skal du rette opp i, i løpet av 6 oppgaver som blir gradvis vanskeligere. Alle funksjonene
du skal fikse på ligger i filen `Game.fs`. Løsningsforslag finnes i branchen `solution`.

## Installasjon

For å kjøre koden i repoet trenger du å ha .NET sdk 8 installert. Instrukser finner du [her](https://learn.microsoft.com/en-us/dotnet/core/install/).
Du kan sjekke at riktig versjon er installert ved å kjøre:
```
$ dotnet --version
```

Når .NET er installert kan du kjøre programmet med følgende kommando fra roten av repoet:
```
$ dotnet run
```

## Funksjonell programmering

I funksjonell programmering jobber vi med verdier som er *immutable* som vi kjører gjennom funksjoner som returnerer nye
verdier som er immutable. Funksjonene forandrer ikke verdien som blir gitt in, men lager en ny verdi, og har oftest ikke
heller noen andre sideeffekter. Dette betyr at du alltid vet hva funksjonen gjør, hvilket gjør det lettere å finne bugs.

**NB:** I funksjonell programmering gir vi argumentene til en funksjon med mellomrom, uten parenteser. For eksempel:
```fsharp
someFun arg1 (someOtherFun arg2 arg3)
```
*ikke*
```fsharp
someFun(arg1, someOtherFun arg2 arg3)
```

Istedet betyr `(arg1, someOtherFun arg2 arg3)` i F# en tupel som inneholder de to verdiene.

Et annet sentralt konsept i funksjonell programmering er *algebraiske datatyper*. I F# har vi *records* og *discriminated
unions*. Disse gjør det lettere å uttrykke det man vil modellere så riktig som mulig.

### Records

Records i F# ligner litt på klasser som kun har felter og ikke metoder i objektorientert programmering. Bruk records når
du vil uttrykke å ha en verdi **og** en annen verdi. Her er et eksempel på en record i F#:

```fsharp
type GameState =
    { CurrentPlayer: Player
      CurrentBoard: Board }
```

Denne typen inneholder to verdier: hvem som er på tur til å spille og hvordan brettet ser ut akkurat nå.

Hvis du har en spiller `player` og et brett `board` så kan du lage en `GameState` med syntaksen

```fsharp
let currentState: GameState =
    { CurrentPlayer = player
      CurrentBoard = board }
```

Dette lager en variabel `currentState` av typen `GameState` der nåværende spiller er `player` og nåværende brett er `board`.
`let` er et keyword som brukes både for å lage en variabel og for å lage en funksjon.

For å hente ut de to verdiene fra variabelen `currentState` bruker du syntaksen

```fsharp
let currentPlayer = currentState.CurrentPlayer
let currentBoard = currentState.CurrentBoard
```

### Discriminated unions

Discriminated unions i F# brukes når man vil uttrykke å ha en verdi **eller** en annen verdi. Her er et eksempel på en
discriminated union:

```fsharp
type Player =
    | X
    | O
```

Denne typen sier at en spiller er én av to mulige verdier: enten er verdien `X` eller så er den `O`.

For å lage en verdi av `Player` velger du bare et av alternativene:

```fsharp
let player = X
```

Dette lager en variabel `player` av typen `Player` som har verdien `X`.

For å bruke en variabel `player` av typen `Player` må du si hva som skal skje for de to forskjellige alternativene:

```fsharp
let showPlayer (player: Player) : string =
    match player with
    | X -> "X"
    | O -> "O"
```

Dette lager en funksjon `showPlayer` som tar en spiller som input og lager tilsvarende streng. Siden `player` er en
discriminated union må vi si hva som skal skje i alle alternativene. Det gjør vi ved å bruke `match ... with` etterfulgt
av `| ... -> ...` for hvert alternativ. Dette er som `if ... then ... else`, bare at istedet for `true` eller
`false` som alternativer har vi `X` eller `O`.

## Oppgave 1

**Oppgave:** Fyll ut funksjonen `otherPlayer`. Den skal returnere motsatt spiller mot hva den får inn. Så hvis den får `X` skal den
returnere `O`, og hvis den får `O` skal den returnere `X`. Tips: bruk `match ... with`.

## Oppgave 2

Så langt ligner dette på Enums i C#. Men vi kan gjøre enda mer med discriminated unions. Vi kan putte forskjellige verdier
inn i de forskjellige alternativene. Her er et eksempel på en slik discriminated union:

```fsharp
type CellState =
    | Empty
    | Taken of Player
```

Dette er statusen på en posisjon på brettet. Den er enten tom, eller så er den tatt av en spiller. I det tilfellet lagrer
vi også hvilken spiller den er tatt av.

For å lage en `CellState` som er tatt av en spiller bruker vi syntaksen:

```fsharp
let cellTakenByX : CellState =
    Taken X
```

For å bruke en variabel av typen `CellState` bruker vi også `match ... with`:

```fsharp
let switchPlayer (cellState: CellState) : CellState =
    match cellState with
    | Empty -> Empty
    | Taken X -> Taken O
    | Taken O -> Taken X
```

Dette er en funksjon som tar som input en `CellState` og hvis den er tom gir tilbake tom `CellState`, eller hvis den er
tatt av `player` gir tilbake en `CellState` som er tatt av motsatt spiller.

Det er også mulig å bruke funksjoner i outputen:

```fsharp
let switchPlayer (cellState: CellState) : CellState =
    match cellState with
    | Empty -> Empty
    | Taken player -> Taken (otherPlayer player)
```

**Oppgave:** Fyll ut funksjonen `showCellState` som tar inn en `CellState` og returnerer en streng som representerer staten. Hvis den
får inn `Empty` skal den returnere `"-"`, og hvis den får inn `Taken player` skal den returnere resultatet av `showPlayer`
brukt på `player`.

## Oppgave 3

Vi kan også ignorere verdien i alternativene:

```fsharp
let takeByX (cellState: CellState) : CellState =
    match cellState with
    | Empty -> Taken X
    | Taken _ -> Taken X
```

**Oppgave:** Fyll ut funksjonen `isTaken`. Den tar inn en `CellState` og hvis den er `Empty` skal funksjonen returnere
`false`, ellers skal den returnere `true`.

## Oppgave 4

I funksjonell programmering bruker vi ikke `null`. Den verdien finnes ikke i F#. Det betyr at når du har en variabel
av typen `string` f.eks. så vet du at den aldri er `null`. Istedet, når vi trenger en variabel som kanskje har en gyldig
verdi, men kanskje ikke, bruker vi i F# typen `Option`. Den er en discriminated union som ser slik ut

```fsharp
type Option<'a> =
    | Some of 'a
    | None
```

Her er `'a` en vilkorlig type. Så du kan for eksempel lage `Option<string>`, `Option<int>` eller `Option<Player>`. For å
bruke en verdi av typen `Option<'a>` bruker vi `match .. with` som vanlig. For eksempel:

```fsharp
let tryGetPlayer (cellState: CellState) : Option<Player> =
    match cellState with
    | Empty -> None
    | Taken player -> Some player
```

Vi kan ha flere rader med kode i hvert case i `match ... with` og vi kan matche på flere ting samtidig. For eksempel:

```fsharp
let takenBySamePlayer (cell1: CellState) (cell2: CellState) : Option<Player> =
    match cell1, cell2 with
    | Taken player1, Taken player2 -> if player1 = player2 then Some player1 else None
    | _ -> None
```

**Oppgave:** Fyll ut funksjonen `allOnePlayer` som sjekker om tre celler er tatt av samme spiller og i det tilfellet
returnerer den spillern, ellers skal den returnere `None`.

## Oppgave 5 🌶️

I funksjonell programmering bruker vi ikke exceptions. Funksjonene vi lager er nesten alltid uten sideefekkter. Istedet
bruker vi i F# typen `Result`. Den er også en discriminated union:

```fsharp
type Result<'a,'e> =
    | Ok of 'a
    | Error of 'e
```

Her må vi gi to typer: `'a` er typen av gyldige verdier og `'e` er typen av feil vi kan få. For eksempel, i vårt spill
har vi feiltypen:

```fsharp
type PositionError =
    | UnknownPosition of string
    | PositionTaken
```

Så hvis vi har en funksjon som skal returnere et brett, men som kan feile med et posisjonsfeil bruker vi typen `Result<Board, PositionError>`.

**Oppgave:** Fyll ut funksjonen `tryPlaceMark`. Den skal sjekke om gitt posisjon er `Empty` og i det tilfelle plassere
gitt spiller der, ellers hvis posisjonen allerede er tatt skal den returnere en `Error` som sier at posisjonen allerede
er tatt `PositionTaken`.

**Tips:** du kan putte et uttrykk, f.eks. resultatet av en funksjon, i `match ... with`. Bruk funksjonene `getCellState` og `placeMark`.

## Oppgave 6 🌶️🌶️

I funksjonell programmering kjører vi gjerne en verdi igjennom flere funksjoner etter hverandre. For å gjøre det kan vi
bruke `|>`:

```fsharp
startValue |> function1 |> function2
```

Her begynner vi med `startValue` som så blir puttet inn i `function1`. Når den er ferdig blir resultatet fra `function1`
direkte puttet inn i `function2` og så blir til sist resultatet fra `function2` returnert. (For at dette skal være mulig
må returtypen fra `function1` være samme som input typen til `function2`.)

**Oppgave:** Fyll ut funksjonen `boardIsFull`. Den skal returnere `true` hvis alle posisjoner er tatt av en eller annen
spiller, ellers `false`.

**Tips:** bruk `allPositions`, `getCellState`, `isTaken` sammen med
[`List.map`](https://fsharp.github.io/fsharp-core-docs/reference/fsharp-collections-listmodule.html#map),
[`List.forall`](https://fsharp.github.io/fsharp-core-docs/reference/fsharp-collections-listmodule.html#forall)
fra F# sitt standardbibliotek.

## Ekstra oppgave 🌶️️🌶️🌶️

Prøv å skrive om programmet med følgende type for brettet:

```fsharp
type Board = Map<Position, Player>
```