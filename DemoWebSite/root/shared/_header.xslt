<xsl:stylesheet version="1.0"
          xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
          xmlns:Url="xrc:UrlModule" exclude-result-prefixes="Url">
  <xsl:output method="html" omit-xml-declaration="yes"/>
  <xsl:param name="activeMenu" />

  <xsl:template match="header">
    <div class="navbar navbar-fixed-top">
      <div class="navbar-inner">
        <div class="container">
          <a class="btn btn-navbar" data-toggle="collapse" data-target=".nav-collapse">
            <span class="icon-bar"></span>
            <span class="icon-bar"></span>
            <span class="icon-bar"></span>
          </a>
          <a class="brand" href="{Url:Content('~')}">xrc demo web site</a>
          <div class="nav-collapse">
            <ul class="nav">
              <xsl:apply-templates select="menu"/>
            </ul>
          </div>
        </div>
      </div>
    </div>
  </xsl:template>
  <xsl:template match="menu">
    <li>
      <xsl:if test="@text=$activeMenu">
        <xsl:attribute name="class">active</xsl:attribute>
      </xsl:if>
      <a href="{Url:Content(@url)}">
        <xsl:value-of select="@text"/>
      </a>
    </li>
  </xsl:template>
</xsl:stylesheet>
