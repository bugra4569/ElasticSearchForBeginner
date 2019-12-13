# ElasticSearchForBeginner
NetCore ElesticSearch For Begginers
Dowloading

Dowload ElesticSearch https://www.elastic.co/downloads/elasticsearch
install and start elasticSearch (on powershell type "curl http://localhost:9200/")


it's start on 9200 port


Note:
Elasticsearch indices have the following naming restrictions:

    *	All letters must be lowercase.

    *	Index names cannot begin with _ or -.

    *	Index names cannot contain spaces, commas, :, ", *, +, /, \, |, ?, #, >, or <.

Don't include sensitive information in index, type, or document ID names. 
Elasticsearch uses these names in its Uniform Resource Identifiers (URIs). 
Servers and applications often log HTTP requests, which can lead to unnecessary data exposure if URIs contain sensitive information