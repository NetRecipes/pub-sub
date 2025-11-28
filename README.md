# Pub / Sub

**.NET Aspire** + **DAPR** - Pub / Sub

```mermaid
sequenceDiagram
    participant ServiceA as ServiceA (Publisher)
    participant SidecarA as Dapr Sidecar (ServiceA)
    participant Broker as Message Broker (Redis/RabbitMQ)
    participant SidecarB as Dapr Sidecar (ServiceB)
    participant ServiceB as ServiceB (Subscriber)

    ServiceA->>SidecarA: Publish event "order"
    SidecarA->>Broker: Deliver event
    Broker->>SidecarB: Distribute event
    SidecarB->>ServiceB: Subscribe with event data
```

## For a written article, refer: [Pub / Sub](https://netrecipes.github.io/courses/dapr-aspire/pub-sub/)

- [x] `Redis Streams`

```yaml
apiVersion: dapr.io/v1alpha1
kind: Component
metadata:
  name: pubsub
spec:
  type: pubsub.redis
  version: v1
  metadata:
  - name: redisHost
    value: localhost:6500
  - name: redisPassword
    value: "localDev"
```

---

- [x] `RabbitMQ`

```yaml
apiVersion: dapr.io/v1alpha1
kind: Component
metadata:
  name: pubsub
  namespace: default
spec:
  type: pubsub.rabbitmq
  version: v1
  metadata:
  - name: host
    value: "amqp://guest:guest@localhost:5672"
```

---
