#### 4.0.0-beta0003 - 02.01.2018
* Unifies the IAggregateRepository and IProjectionRepository with the interface IRepository

#### 4.0.0-beta0002 - 17.08.2017
* Uses official netstandard20

#### 4.0.0-beta0001 - 27.07.2017
* Transitioning to netstandard20

#### 3.2.2 - 04.07.2017
* Fixes ProjectionDefinition

#### 3.2.1 - 04.07.2017
* Moves IProjectionRepository from Cronus.Projections.Cassandra

#### 3.2.0 - 04.07.2017
* Replace projections with eventsourced projections

#### 3.1.4 - 23.09.2016
* Adds the AssemblyInfo file to the project

#### 3.1.3 - 20.09.2016
* Removes the IMessage.ToString(template,params) signature. It is a breaking change but the version will not be increased.

#### 3.1.2 - 14.09.2016
* Implements exception throw if cannot parse StringTenantUrn

#### 3.1.1 - 14.09.2016
* Adds StringTenantUrn. Handy!

#### 3.1.0 - 08.09.2016
* Adds EntityStringId
* Adds IPublisher<IScheduledMessage> TimeoutRequestPublisher to the ISaga interface
* Improve the URN
* Removes Message class

#### 3.0.2 - 08.06.2016
* Fix StringId to set properly the Urn
* Fix GuidId to set properly the Urn
* Fix GuidTenantId to set properly the Urn
* Fix EntityGuidId to set properly the Urn
* Fix AggregateRootId to set properly the Urn
* Introduce EntityStringId

#### 3.0.1 - 02.06.2016
* Fix GuidId to set properly the RawId

#### 3.0.0 - 10.05.2016
* Breaking changes: RawId is assembled in different way. You need to migrate your old RawIds to the new ones.
* IProjectionCollectionState now inherit IProjectionState
* Introduce ProjectionState
* IKeyValueCollectionPersister updated to support retrieving single collection item
* Introduce UrnId

#### 2.4.0 - 26.03.2016
* Introduce GuidTenantId and StringTenantId

#### 2.3.0 - 19.03.2016
* Improve the IPublisher interface with ability to schedule messages in future
* Introduce experimental interfaces for Saga support

#### 2.2.1 - 04.09.2015
* Replace the exception when a state handler is missing

#### 2.2.0 - 08.07.2015
* Added persistent projection abstraction

#### 2.1.6 - 06.07.2015
* Make entity event serializable

#### 2.1.5 - 06.07.2015
* Properly apply events which are part of an entity

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
