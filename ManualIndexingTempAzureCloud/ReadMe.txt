ManualIndexingTemp
------------------

Purpose
-------
Lab requisitions are primarily indexed by barcode.
For cases where there is no readable barcode, this allows the scan operator to manually enter the indexing information.

Usage
-----
Typically called from Scanquire
Command Line: ManualIndexingTemp.exe /batchid:[Batch Id]

Configuration
-------------
DefaultScanStationID: Not Used
DefaultIndexNumber: Deprecated
OldIndexNumberRegex: Match pattern for old format valid index values
OldIndexNumberMessage: Confirmation message to be displayed if an old format index value is entered by the user.
ValidIndexNumberRegex: Match pattern for new format valid index values.
ArchiveRoot: Folder path to store the batch to after indexing
UploadToOptix: True to enable uploading to Optix
UploadToSP: True to enable uploading to SharePoint
UploadToOptixScriptPath: Location of the optix import script
UploadToSPScriptPath: Location of the sharepoint import script
ValidRequisitionNumberRegex: Match pattern for validating Requisition numbers