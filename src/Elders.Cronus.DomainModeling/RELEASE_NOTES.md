#### 2.1.4 - 06.07.2015
* Creating an entity now requires an ID

#### 2.1.3 - 06.07.2015
* Creating an entity now requires an ID

#### 2.1.2 - 06.07.2015
* Properly register entity event handler

#### 2.1.1 - 06.07.2015
* Fix bug which does not allow more than one entity per Aggregate

#### 2.1.0 - 02.07.2015
* Use generic class for EntityId

#### 2.0.4 - 02.07.2015
* Use ByteArrayHelper everywhere

#### 2.0.3 - 02.07.2015
* Fix issue with EntityId class

#### 2.0.2 - 02.07.2015
* Use IBlobId for the RawId

#### 2.0.1 - 02.07.2015
* Roll back the IAggregateRootId

#### 2.0.0 - 02.07.2015
* Add reference in the AR state back to the root
* Add EntityGuidId
* Change the IEntityState interface to not be a generic one
* Add Entity classes similar to the AggregateRoot

#### 1.3.0 - 12.06.2015
* Add method TryLoad when loading aggregates from the event store

#### 1.2.3 - 23.03.2015
* Add Message object to wrap the Commands and the Events. Message has headers.

#### 1.2.2 - 23.03.2015
* Caching different handlers is not based on ICommand/IEventHandler interfaces but IProjection, IPort, IAppService

#### 1.2.1 - 15.01.2015
* Improve .ToString() output for AggregateRootId classes

#### 1.2.0 - 16.12.2014
* Add IProjection interface
* Add aggregate root name to the IAggregateRootId interface
* Remove TransportMessage
* Initizalize Aggregate root state instance with history events
* Move the revision from the state to the aggregateRoot
* Properly calculate GetHashCode for RawId
* Initialize empty AggregateRootId with empty RawId

#### 1.0.3 - 01.10.2014
* Rework how we use the aggregate Ids
* Bug fix

#### 1.0.2 - 01.10.2014
* Rework how we use the aggregate Ids

#### 1.0.1 - 29.09.2014
* Remove DomainMessageCommit

#### 1.0.0 - 10.09.2014
* Initial version (moved from Cronus.DomainModelling)