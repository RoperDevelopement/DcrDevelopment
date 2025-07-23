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
          text-align: center;
          background-color:yellow;
          }
          h1 {
          text-align: center;
          }
          body {
          background-color: #edf0f5;
          }
        </style>

      </head>
      <body>
        <h1>BinMonitor Report Created at {dateTime}</h1>
        <xsl:for-each select= "/Bins/LabReqs">
          <div>

           

            <table width="100%" style="" >
              <caption>BinID  <xsl:value-of select="BinID"/> LabReq Number  <xsl:value-of select="LabReqNumber"/> Category <xsl:value-of select="CategoryName"/></caption>
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
                  <xsl:value-of select="regCompletedBy"/>
                </td>
                <td>
                  <xsl:value-of select="regCompletedAt"/>
                </td>
                <td>
                  <xsl:value-of select="regDuration"/>
                </td>
              </tr>
              <tr>
                <td>
                  <xsl:value-of select="Processing"/>
                </td>
                 <td>
                  <xsl:value-of select="processAssignedBy"/>
                </td>
                <td>
                  <xsl:value-of select="processAssignedTo"/>
                </td>
                <td>
                  <xsl:value-of select="processStartAt"/>
                </td>
                <td>
                  <xsl:value-of select="processCompletedBy"/>
                </td>
                <td>
                  <xsl:value-of select="processCompletedAt"/>
                </td>
                <td>
                  <xsl:value-of select="prossDuration"/>
                </td>
               
              </tr>
              <tr>
                <td>
                  <xsl:value-of select="BinClosed"/>
                </td>
                <td>
                  <xsl:value-of select="regCreatedBy"/>
                </td>
                <td>
                  <xsl:value-of select="regCreatedBy"/>
                </td>
                <td>
                  <xsl:value-of select="regStartedAt"/>
                </td>
                <td>
                  <xsl:value-of select="binClosedBy"/>
                </td>
                <td>
                  <xsl:value-of select="binCompletedAt"/>
                </td>
                <td>
                  <xsl:value-of select="totalDuration"/>
                </td>

              </tr>

            </table>
            <br/>
            <p>Comments: <xsl:value-of select="binComments"/> </p>
            <br/>
          <p>Contents: <xsl:value-of select="Contents"/> </p>
          <br/>
          </div>
        </xsl:for-each>
        <!--<-->
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>
