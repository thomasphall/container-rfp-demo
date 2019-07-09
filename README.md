# container-rfp-demo

A sample application for use in testing container solutions.

This solution, taken in its entirety, provides a fake stress-tess application for putting a load on RabbitMQ brokers and determining in messages are dropped under load.



## Applications
The solution consists of four applications:

### Publisher
Publisher is an application that performs the following actions:
1. Publishes one *EventMessage* per second, and has no knowledge of any subscribers to that event.
2. Puts the *EventMessage's* unique identifer in the *unconsumedmessage* table so we can check later to see if any messages were not properly handled.

### Subscriber
Subscriber is an application that performs the following actions:
1. Subscribers to the *EventMessage* event, and consumed *EventMessages* as they are published with a maximum throughput of one message per second.
2. Removes the *EventMessage's* unieque identifer from the *unconsumedmessage* table so we can check later to see if any messages were not properly handled.

### Statistics
Statistics is a web API endpoint with a single purpose:
- It provides callers with the current message count for the given RabbitMQ queue name.

### Website
Website is an ASP.Net Core MVC application with a single purpose:
- It allows users to see the current message count for the Subscriber queue in RabbitMQ.  This allows us to easily inspect whether there are currently enough *Subscribers* to keep up with the active *Publishers*.



## Usage

### Prerequisites
- A host machine with the following:
    - A Docker-Enabled host capable of running Docker Compose applications compatible with the 3.4 Docker Compose format or greater.
    - An internet connection with access to Git Hub and Docker Hub.
    - A git client.

### Getting the application
- Navigate to the directory where you want the solution to be downloaded.  We'll use C:\src as an example:
          cd C:\src
- and clone the source code from Git Hub with the following git command:
    git clone https://github.com/EnterpriseProductsLP/container-rfp-demo.git

