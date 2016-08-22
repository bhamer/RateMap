#####Description
Trie-like data structure for finding a best-match rate to charge given a set of attributes. For example, if a transaction can have attributes { A, B, C, D }, the data structure will take a transaction with it's corresponding {A, B, C, D} attribute values and return the appropriate rate to charge based on those values.

The attributes are ranked and there doesn't need to be a perfect match. For example, given attributes { A, B, C, D } ordered by rank from left to right. If attribute A matches, it would be a better match than matching on attributes B, C, and D but not A.

#####Requirements
Fast search time. Search will be called many times in loop.

#####Example
The rate table below has attributes { A, B, C, D } ordered in rank from left to right and the associated rate to charge. Find a best-match rate for the set of attribute values { 1, 5, 3, 2 }.
| A | B | C | D | Rate |
|---|---|---|---|------|
| 1 | 1 | 1 | 4 | .01  |
| 1 | 2 | 1 | 2 | .02  |
| 1 | 1 | 3 | 1 | .03  |
| 2 | 4 | 4 | 4 | .005 |
 
 The best match would be { 1, 1, 3, 1, .03 } since this { 1, *, 3, * } is a better match than { 1, *, *, 2 } due to attribute C being a higher rank than attribute D.
 
#####Performance
where n = # of rates and k = # of attributes associated with rates (k = 4 as it is now)
Search: O(k)
Build: O(n * k)
Space: O(n * k)