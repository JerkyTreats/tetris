# Tetris

We all know it. Pretty dope stuff.

Uses [SRS](https://tetris.fandom.com/wiki/SRS) rotation, the rotation system users didn't realize they want. Handles wallkicks.

Basically an exact copy of the (amazing) [YouTube tutorial](https://www.youtube.com/watch?v=ODLzYI4d-J8) by [Zigurous](https://github.com/zigurous/). This includes their assets and copypasta Tetromino data.

## Current Next Steps

There's a couple things here that's top of mind:

* Continue Refactor
  * Undo ActivePiece stuff
  * Introduce Observer Pattern to GameController
* Board Serialization to file
  * Been working towards this in a big way
  * Connect Save / Load to Protobuf serialization engine
  * Just saving to disk is a good start
* Figure out how to have a UI spanning multiple scenes
  * Give that UI the ability to load/unload different scenes
  
# Goals

There are a bunch of different goals I'm working on for this prototype. Maximum learning and experimentation is top-of-mind, all the while pushing to become a better developer.

### Major Verticals

* Completed prototype gameplay of the Core Toy
  * Prototype extensibility and multiple rulesets
* Shared interface with a backend 
  * Backend into DB, cache, etc. 
  * Admin + Ops 
* Metagame 

## Design Goals

- Load boards with blocks already placed.
  - Requires De/Serialization
  - I'm going hard mode with `protobuf` serialization
    - Might into gRPC eventually
- Stitch multiple boards together.
  - With dynamic camera, borders, and grid assets, this should be relatively easy to implement
- After clearing arbitrary amount from current board, move to the next board.
- Complete all boards to complete game.

- Add special pieces
  - Bombs that explode the bricks
  - Unstable Tetrominos... also explode :D
  - Laser that explodes some blocks
  - You get the idea

- Metagame considerations
  - Blocks on destruction have chance of becoming rewards
    - They fly up into some rewards buckets
    - Once complete all boards, recieve awards in the bucket

Combining the two ideas makes it about finding entertaining ways to explode the shit out of blocks.

## Programming Goals

### Large

* _Board Editor (in progress)_
* Board Serialization (eventually into gRPC?)
* Stitching boards together
* Start thinking about MetaGame
* Rebuild input system for mobile

### Small
- Score/Reward
- Scaling Speed on line(s) cleared.

## Completed Goals

- Complete Tutorial
- Dynamic Border
- Dynamic Background Grid
- Add proper new peice queue (A bag containing each piece randomly chosen until every piece chosen, then create new bag). "Real" Tetris shouldn't be full random Piece selection.
- Dynamic Camera
- Mutliple Boards

## Known Bugs

- ~~Ghost piece shouldn't search from bottom up, it should start from top and search down.~~
  - If the `I` piece is the first to spawn, its Ghost will be placed in an incorrect position.
  - The ghost for `I` in general seems a bit messed up. I think its because its the only piece that is one size high.
