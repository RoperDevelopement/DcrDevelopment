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
                    <h1>BinMonitor Changes Report Created at {dateTime}</h1>

        <div>



          <table style="width:100%">
            <caption>
              BinMonitor Changes
            </caption>
            <tr>
              <th>Id</th>
              <th>EmailAddress</th>
              <th>EmailSubject</th>
              <th>CreateDate</th>
              <th>Comments</th>
            </tr>
            <xsl:for-each select= "BinMonitor/Changes">
              <tr>
                <td>
                  <xsl:value-of select="Id"/>
                </td>
                <td>
                  <xsl:value-of select="UserEmailAddress"/>
                </td>
                <td>
                  <xsl:value-of select="EmailSubject"/>
                </td>
                <td>
                  <xsl:value-of select="DateCreated"/>
                </td>
                <td>
                  <xsl:value-of select="Comments"/>
                </td>
                
              </tr>
            </xsl:for-each>
          </table>


        </div>
        
        <!--<-->
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>
