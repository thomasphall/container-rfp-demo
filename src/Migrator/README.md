# Migrator

Migrator is a tool that runs database migrations on starup.

Since this is all a fake tool, it also deletes and recreates the target database so we start with a clean slate each time our containers are started.

It shuts down on completion.
