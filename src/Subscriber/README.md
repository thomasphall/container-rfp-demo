# Subscriber

Subscriber is a fake tool for stress-testing RabbitMQ.  It consumes one message per second, and the unique identifier of the message is also removed from the "UnconsumedMessage" table in SQL Server.

If any message is not consumed by an appropriate Subscriber, it remains in the table.  If all messages are drained from the "Subscriber" queue in RabbitMQ, we will know the message remaining in the "unconsumed" message table have been dropped.
