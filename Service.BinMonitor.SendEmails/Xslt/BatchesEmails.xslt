<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">
  <xsl:output method="html" indent="yes"/>

  <xsl:template match="/">
    <html lang="en" xmlns="http://www.w3.org/1999/xhtml">
      <head>
        <meta charset="utf-8" />
        <title>BinMonitor Email</title>

        <style>
          table, th, td {
          border: 1px solid black;
          }
          h1 {
          text-align: center;
          }
          body {
          background-color: LightGray;
          }
        </style>

      </head>
      <body>
                    <h1>{dateTime}</h1>
        <xsl:for-each select= "BatchId/Bins">
          <div>



            <table style="width:100%">
              <caption>
                 Spectrum Id  <xsl:value-of select="batchId"/>
              </caption>
              <tr>
                <th>Bin Status</th>
                <th>Assigned By</th>
                <th>Assigned To</th>
                <th>Started At</th>
                <th>Completed By</th>
                <th>Completed At</th>
                <th>Duration</th>
              </tr>
              <tr>
                <td>
                  <xsl:value-of select="Registation"/>
                </td>
                <td>
                  <xsl:value-of select="regCreatedBy"/>
                </td>
                <td>
                  <xsl:value-of select="regAssignedTo"/>
                </td>
                <td>
                  <xsl:value-of select="regStartedAt"/>
                </td>
                <td>
                  <xsl:value-of select="regCompltedBy"/>
                </td>
                <td>
                  <xsl:value-of select="regCompletedAt"/>
                </td>
                <td>
                  <xsl:value-of select="regDuration"/>
                </td>
              </tr>
            </table>          
          </div>
        </xsl:for-each>
        
        <!--<-->
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>
