#Design
![picture](img/diagram.gif)
#Decisions and assumptions
To keep request rate under the limit and because we have multiple service nodes, worker process was introduced.
Also worker will help if external service is offline. We can use existing data.

### Worker process - ExchangeRateDownloadService
Read new rates from external api based on schedule and save exchange pairs into storage.
Based on fixer:
    -1.000 API Calls
    -Hourly Updates
Service will read data once an hour. In this case it will be 720 call per month.

Conversion rate should be with 5 decimal places precision

For Windows we can use Schedule task

###Storage
For simplicity i will use network folder share with files. 
Each hour service will download and create new file with new rates with tmp extension and after writing completed will rename file to txt extension.
File name format: exchangerates_2018111013.txt

Fututre To Do: Another service which will cleanup or archived the files after one day should be introduced.
To make storage work default user temp directory should be accessible for tests and user account used to run Worker and Web Api Services

###Service node - ExchangeRateService
Self hosted rest service in console. 
Upon request read data from file share (last file by date with txt extension).
Will cache information for 5 minutes in local cache.
Use swagger for service description.

# Requirements
# Exchange Rate Service

In order for a Bookmaker to see their liabilities in a common currency, An exchange rate microservice is required. This exchange rate service will be responsible for collection and delivery of exchange rate values to internal clients. 

#### It must: ####

- Provide a RESTful api to fetch an exchange rate for a specified _base currency_ and _target currency_;
- The most recent exchange rate for the requested currency exchange should be returned;
- Converstion rate precision is to 5 decimal places;
- Any design decisions and assumptions should be documented.


#### Implementation: ####

The solution must conform to the below requirements:

- C#/ Visiual Studio 2015+;
- Leverage a DI Container of your choice;
- Utilize open-source/ free nuget packages/ extensions (if applicable);
- Provide a RESTful endpoint as described below;
- Retrieve exchange rates from the free api https://fixer.io/


#### Data Source ####

Your solution must acquire conversion rates via the REST api provided by fixer.io. 

The cartesian product excluding matching _base_ and _target_ pairs of the following currencies should be supported:

-        AUD;
-        SEK;
-        USD; 
-        GBP;
-        EUR.

Extensibility should be considered.

#### REST Api: ####

##### Retrieve Exchange Rate #####

The api must support a GET HTTP verb with the following contract:



```json

{
    "baseCurrency": "GBP",
    "targetCurrency": "AUD"
}

```
where

- baseCurrency : string, required;
- targetCurrency : string, required;


with response:



```json

{
    "baseCurrency": "GBP",
    "targetCurrency": "AUD",
    "exchangeRate" : 0.7192,
    "timestamp" : "2018-03-21T12:13:48.00Z"
}

```

#### NFR's ####

- Real-time exchange rates are not required, prices are used for estimation only;
- Expected load on this API is ~5 - 15 requests/ sec;
- Deployed to a load-balanced environment (~2-3 nodes).


#### Submission ####

For a submission to be considered complete it **must**:

 - Demonstrate appropriate use of source control/ versioning;
 - Adhere to modern coding standards/ practices;
 - Be merged to master with a merge commit;
 - Be testable;
 - Solve the problem.

This repository should be _private cloned_ and shared with _techtest_au@kindredgroup.com_ when complete.


#### Hints ####
* Free fixer.io api does not support base currencies other than _EUR_;
* Be aware of the free api limitations.

