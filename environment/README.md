# Environment Manipulation

Provides `loadEnvironment(string file)` which mirrors `saveEnvironment(string file)`, as well as a `setEnvironment(string varName, value)` function which behaves identically to `serverCmdSetEnvironment(GameConnection client, string varName, value)` but does not require a client with admin rights.
