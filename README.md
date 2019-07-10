# container-rfp-demo Solution

A sample application for use in testing container solutions.

This solution, taken in its entirety, provides a fake stress-tess application for putting a load on RabbitMQ brokers and determining if messages are dropped under load.



## Prerequisites
- A host machine with the following:
    - A Docker host capable of running Docker Compose applications compatible with the 3.4 Docker Compose format or later.
    - An internet connection with access to Git Hub and Docker Hub.
    - A git client.
    - Nothing currently bound on the following local TCP ports:
        - **1433**     (*SQL Server*)
        - **5672**     (*RabbitMQ*)
        - **8080**     (The *Website* application)
        - **8081**     (The *Statistics* application)
        - **15672**    (*RabbitMQ*)



## Applications
The solution consists of four applications:


### *Publisher*
*Publisher* is an application that performs the following actions:
1. Publishes one *EventMessage* per second, and has no knowledge of any *Subscribers* to that event.
2. Puts the *EventMessage's* unique identifer in the *UnconsumedMessage* table so we can check later to see if any messages were not properly handled.


### *Subscriber*
*Subscriber* is an application that performs the following actions:
1. Subscribes to the *EventMessage* event, and consumed *EventMessages* as they are published with a maximum throughput of one message per second.
2. Removes the *EventMessage's* unieque identifer from the *UnconsumedMessage* table so we can check later to see if any messages were not properly handled.


### *Statistics*
*Statistics* is a web API endpoint with a single purpose:
- It provides callers with the current message count for the given RabbitMQ queue name.


### *Website*
*Website* is an ASP.Net Core MVC application with a single purpose:
- It allows users to see the current message count for the *Subscriber* queue in RabbitMQ.  This allows humans to easily inspect whether there are currently enough *Subscribers* to keep up with the active *Publishers*.



## Dependencies
The solution has dependencies on two public Container images:


### SQL Server (Linux)
Image Name:  mcr.microsoft.com/mssql/server

Used for: Tracking which published *EventMessages* have not yet been consumed by *Subscribers*

### RabbitMQ
Image Name:  rabbitmq:3-management

Used for:  An Enterprise-Messaging transport.  This is what we are pretending to stress-test.



## Usage


### Getting the application
Navigate to the directory where you want the solution to be downloaded.  We'll use **~/source** as an example:
~~~
cd ~/source
~~~

Clone the source code from Git Hub with the following git command:
~~~
git clone https://github.com/EnterpriseProductsLP/container-rfp-demo.git
~~~

The source code is downloaded to **~/source/container-rfp-demo**.


### Running the application
Navigate to the directory where you cloned the source code, then into the "src" directory.  We'll use **~/source/container-rfp-demo/src** as an example:
~~~
cd ~/source/container-rfp-demo/src
~~~

You are now in the folder containing the docker-compose.yml file.  To run the solution, execute the following command from your prompt:
~~~
docker-compose up -d
~~~

This command will perform the following actions, running the solution in the detached state:
- Download all necessary docker images from docker hub.
- Build new docker images for the *Publisher*, *Subscriber*, *Statistics*, and *Website* applications.
- Run the application dependencies:  RabbitMQ and SQL Server
- Run the *Migrator* application, which will:
    - Destroy any existing *RabbitStressTest* database instance.
    - Create the *RabbitStressTest* database.
    - Run the necessary migrations against the *RabbitStressTest* database to create tables and stored procedures for our stress-test application.
- Run the solution applications:
    - *Publisher*
    - *Subscriber*
    - *Statistics*
    - *Website*



## Checking That the Application is Running Correctly


### Checking the *Publisher* and *Subscriber*
From a command prompt in the same directory where you ran docker-compose, execute the following command:
~~~
docker-compose logs -f
~~~

This will follow the log output for all of the running solution applications and emit them in your command prompt.  If all is well, you should see no errors.  You should also see log messages similar to the following:
~~~
publisher_1   | Published message: becb8440-0c14-4b0b-ad95-4523606210e2
publisher_1   | Published message: fe2d4b2b-15a0-4ffd-9354-fd584ddc2ed0
subscriber_1  | Consumed message: becb8440-0c14-4b0b-ad95-4523606210e2
publisher_1   | Published message: 42d76cbf-efeb-43af-888f-622c2ac1661d
subscriber_1  | Consumed message: fe2d4b2b-15a0-4ffd-9354-fd584ddc2ed0
subscriber_1  | Consumed message: 42d76cbf-efeb-43af-888f-622c2ac1661d
publisher_1   | Published message: b663179e-fe02-4110-9bc5-1e9d3f1f9727
~~~

These messages indicate that the configured *Publisher* and *Subscriber* are publishing and consuming messages.


### Checking the *Statistics* Site
You can confirm the *Statistics* site is running correctly by making an http ***GET*** request to the endpoint.  You can use curl, PostMan, your favorite browser, or any other http client that you like.

Make a request to the following URL:  http://localhost:8081/api/QueueDepths/Subscriber

Curl example:
~~~
curl http://localhost:8081/api/QueueDepths/Subscriber
~~~

Example output:
~~~
StatusCode        : 200
StatusDescription : OK
Content           : 0
RawContent        : HTTP/1.1 200 OK
                    Transfer-Encoding: chunked
                    Content-Type: application/json; charset=utf-8
                    Date: Tue, 09 Jul 2019 16:59:07 GMT
                    Server: Kestrel

                    0
Forms             : {}
Headers           : {[Transfer-Encoding, chunked], [Content-Type, application/json; charset=utf-8], [Date, Tue, 09 Jul 2019 16:59:07 GMT], [Server, Kestrel]}
Images            : {}
InputFields       : {}
Links             : {}
ParsedHtml        : mshtml.HTMLDocumentClass
RawContentLength  : 1
~~~

You need to verify two things:
1. The *StatusCode* should be **200**.
2. The *Content* should be **an integer >= 0**


### Checking the *Website*
You can confirm the *Website* is running by making an http ***GET*** request to the endpoint.  Open the following URL in your favorite browser:  http://localhost:8080/

You should see three things:
1.  A page header with some (pretty useless) links.
2.  A section header named "System Health"
3.  An "Unconsumed Message" count showing an integer value of zero or near-zero.



## Experimenting With *Publisher* Load and *Subscriber* Capacity
<span style="color:red">***In your live demonstration using Kubernetes, we would like you to demonstrate that the number of running *Subscribers* can be increased automatically as the number of running *Publishers* is increased manually.***</span>

<span style="color:red">***After testing in your local environment to confirm that the application behaves as I claim it will, we would like you to create a deployment solution for your platform to demonstrate that automatic scaling of the *Subscriber* application works correctly.***</span>

Using the provided docker-compose.yml file, the *Publihser* load and *Subscriber* capacity are in perfect balance, each publishing or consuming one message per second.  This means the message count will stay, as it should, near zero at all times.

In order to demonstrate your ability to auto-scale the number of running *Subscribers* based on the count of unconsumed messages, you will first need to run additional *Publishers* to create an imbalance between publishing load and consuming capacity.  This can be accomplished by running additional *Publihsers*.

Once you have created an inbalance, and begin to see numbers of unconsumed messages significantly greater than zero, you will want to add additional *Subscribers* in order to bring the solution's overall consumption capacity above parity with its publishing capacity.

Ideally, in your testing, you would:
1. Confirm that the default configuration provides capacity parity.
2. Add additional *Publihsers* to confirm that the unconsumed message count grows boundlessly, which would eventually push RabbitMQ (or any other transport) beyond its ability to store new messages.
3. Add additional *Subscribers* to confirm that doing so reduces the unconsumed message count back to, or near, zero.

### Increasing *Publisher* Load

To add *Publishers*, add additional "publisher" service sections to the docker-compose.yml file.  The original section looks like the lines below.  You can simply copy/paste a new copy of it, and change the service name:
~~~
  # RENAME THE FOLLOWING LINE:
  publisher:
    image: ${DOCKER_REGISTRY-}publisher
    build:
      context: .
      dockerfile: Publisher/Dockerfile
    depends_on:
      - migrator
      - rabbitmq
      - sqlserver
      - subscriber
~~~

For example, to have three *Publishers* instead of the original single *Publisher*, you might have the following in the services section of your docker-compose.yml file.
~~~
  publisher1:
    image: ${DOCKER_REGISTRY-}publisher
    build:
      context: .
      dockerfile: Publisher/Dockerfile
    depends_on:
      - migrator
      - rabbitmq
      - sqlserver
      - subscriber

  publisher2:
    image: ${DOCKER_REGISTRY-}publisher
    build:
      context: .
      dockerfile: Publisher/Dockerfile
    depends_on:
      - migrator
      - publisher1
      - rabbitmq
      - sqlserver
      - subscriber

  publisher3:
    image: ${DOCKER_REGISTRY-}publisher
    build:
      context: .
      dockerfile: Publisher/Dockerfile
    depends_on:
      - migrator
      - publisher1
      - publisher2
      - rabbitmq
      - sqlserver
      - subscriber
~~~


### Increasing *Subscriber* Capacity

To add *Subscribers*, add additional "subscriber" service sections to the docker-compose.yml file.  The original section looks like the lines below.  You can simply copy/paste a new copy of it, and change the service name:
~~~
  # RENAME THE FOLLOWING LINE:
  subscriber:
    image: ${DOCKER_REGISTRY-}subscriber
    build:
      context: .
      dockerfile: Subscriber/Dockerfile
    depends_on:
      - migrator
      - rabbitmq
      - sqlserver
~~~

For example, to have three *Subscribers* instead of the original single *Subscriber*, you might have the following in the services section of your docker-compose.yml file.
~~~
  subscriber1:
    image: ${DOCKER_REGISTRY-}subscriber
    build:
      context: .
      dockerfile: Subscriber/Dockerfile
    depends_on:
      - migrator
      - rabbitmq
      - sqlserver

  subscriber2:
    image: ${DOCKER_REGISTRY-}subscriber
    build:
      context: .
      dockerfile: Subscriber/Dockerfile
    depends_on:
      - migrator
      - rabbitmq
      - sqlserver
      - subscriber1

  subscriber3:
    image: ${DOCKER_REGISTRY-}subscriber
    build:
      context: .
      dockerfile: Subscriber/Dockerfile
    depends_on:
      - migrator
      - rabbitmq
      - sqlserver
      - subscriber1
      - subscriber2
~~~



## Final Notes


### HTTP/HTTPS
The *Statistics* and *Website* applications are unsecured, and allow HTTP connections.  HTTPS is not enabled.

### Credentials
All of the credentials are the same:

**Username**:  admin

**Password**:  yourStrong(!)Password


### Exposed Service Ports
All of the application ports are exposed and available to standard clients.

#### Connecting to SQL Server
You can use SQL Server Management Studio to connect to the SQL Server container.

**Hostname**:  localhost or .

**Port**:  1433

#### Connecting to RabbitMQ
You can use a browser to connect to the management interface:  http://localhost:15672/

#### Connecting to the *Website* UI
You can use any browser to connect to the *Website* UI:  http://localhost:8080/

#### Connecting to the *Statistics* API
You can use any HTTP client to make a ***GET*** request to to the *Statistics* API.  There is only API call of interest:  http://localhost:8081/api/QueueDepths/Subscriber


### Getting Unconfirmed Message Counts for Dynamic *Subscriber* Scaling
There are two ways your dynamic scaling solution can determine the unconsumed *EventMessage* count to decide it more *Subscribers* need to be started:
1.  You can make an HTTP ***GET*** request to the *Statistics* API endpoint to get the count.  As currently configured, the URLs are:
      - Internal to the application:  http://statistics/api/QueueDepths/Subscriber
      - From the host computer:  http://localhost:8081/api/QueueDepths/Subscriber
2.  You can use RabbitMQ's API endpoint directly.  The documentation is located at:  http://localhost:15672/api
