# Covid Test & Trace Simualtor
How does it all hang together?
1. Service Worker to simulate test and trace centre
1. Covid Testing Lab API (where T&TC or Home Testing kits are sent)
    - Promise API to add the request to send
1. Covid Testing Lab Consumer
    - Runs the simulation covid test
    - Publishes any events
1. Names API (to simulate the NHS DB for people) [X]
1. Test & Trace API (To contact positive and gather list of contacts)
1. Positive contact consumer to tell people to isolate and get tested
1. Government Advice (for getting lockdown information e.g. length of lockdown, lockdown level to use in calculating the chances of having covid) [X]
1. Goverment Expert Worker (To look at the current data and decide lockdown levels etc.)
1. Time/Date Service to replicate the state or provide a mock up
1. Consider a TimeDate Worker that stores the Current Simulated Date in State

## Application Stack
- Redis for quick and easy cache (Promis patterns)
- Rabbit MQ (or MassTransit if Dapr allows) with strict Message policies for all events from API to consumer
- EventStore for Recording Covid Results (projections for gov scientists to use)
    - Publish an event for every covid test w/ nhs number, DateTime and result
    - Projections for Every Day since
- Mongo DB for NHS database
- Sql Server for Covid Contacts
- Elastic Search and Kibana for Logging
- Grafana with Prometheus for metric dashboards
- Email Server to simulate an email mailbox

## Logging
- How many positive cases are there?
- What is the average number of contacts
- 7 day average cases
- Rise/Decline based on prev day/week/month/day of week/week of month

## Metrics
- Length of things like the testing lab
- How many failed requests
- Count of people presenting at Test and Trace/ Home Testing
- Count of positive/negative cases

## Names API
Return a Mock person
## Gov Advice API
Return current advice on length of isolataion/lockdown level
## Gov Expert Worker
Assess the current data to drive the advice
## Test and Trace Consumer
Simualte a person going through a test and trace
## Test and Trace Worker
Post Person data to a queue for the consumer to read
## Covid Testing lab
The API to simualte the sending of covid kits for the labs to assess
## Positive case Test and Trace Consumer
Consumer to collect Positive cases and contact/collect contacts
## Negative case Test and Trace Consumer
Consumer to contact with a negative result
##