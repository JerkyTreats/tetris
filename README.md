# Tetris

We all know it. Pretty dope stuff.

Uses [SRS](https://tetris.fandom.com/wiki/SRS) rotation, the rotation system users didn't realize they want. Handles wallkicks.

Basically an exact copy of the (amazing) [YouTube tutorial](https://www.youtube.com/watch?v=ODLzYI4d-J8) by [Zigurous](https://github.com/zigurous/). This includes their assets and copypasta Tetromino data.

## Goals

- ~~Complete Tutorial~~
- ~~Dynamic Border~~
- ~~Dynamic Background Grid~~
- ~~Add proper new peice queue (A bag containing each piece randomly chosen until every piece chosen, then create new bag). "Real" Tetris shouldn't be full random Piece selection.~~
- ~~Dynamic Camera~~
- ~~Mutliple Boards~~
- Score/Reward
- Scaling Speed on line(s) cleared.

### Design Goals

- Load boards with blocks already placed.
  - Requires De/Serialization
  - I'm going hard mode with `protobuf` serialization
    - Might into gRPC eventually
- Stitch multiple boards together.
  - With dynamic camera, borders, and grid assets, this should be relatively easy to implement
- After clearing arbitrary amount from current board, move to the next board.
- Complete all boards to complete game.

- Add special peices
  - Bombs that explode the bricks
  - Unstable Tetrominos... also explode :D
  - Laser that explodes some blocks
  - You get the idea

- Metagame considerations
  - Blocks on destruction have chance of becoming rewards
    - They fly up into some rewards buckets
    - Once complete all boards, recieve awards in the bucket

Combining the two ideas makes it about finding entertaining ways to explode the shit out of blocks.

## Known Bugs

- ~~Ghost piece shouldn't search from bottom up, it should start from top and search down.~~
  - If the `I` piece is the first to spawn, its Ghost will be placed in an incorrect position.
  - The ghost for `I` in general seems a bit messed up. I think its because its the only piece that is one size high.
