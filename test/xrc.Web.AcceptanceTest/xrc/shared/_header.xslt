<xsl:stylesheet version="1.0"
          xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
          xmlns:Url="xrc:UrlModule"
          xmlns:Page="xrc:PageModule"
		  exclude-result-prefixes="Url Page">
	<xsl:output method="html" omit-xml-declaration="yes"/>
	<xsl:param name="activeMenu" />

	<xsl:template match="header">
		<div class="navbar navbar-fixed-top">
			<div class="navbar-inner">
				<div class="container-fluid">
					<a class="btn btn-navbar" data-toggle="collapse" data-target=".nav-collapse">
						<span class="icon-bar"></span>
						<span class="icon-bar"></span>
						<span class="icon-bar"></span>
					</a>
					<a class="brand" href="{Url:Content('~')}">xrc test</a>
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
		<xsl:variable name="pageUrl" select="Url:Content(@url)" />
		<xsl:variable name="pageTitle" select="Page:GetPageParameterXslt($pageUrl, 'title')" />

		<li>
			<xsl:if test="$pageTitle = $activeMenu">
				<xsl:attribute name="class">active</xsl:attribute>
			</xsl:if>
			
			<!-- This is another way to check if the current page belongs to the specified page -->
			<!--<xsl:if test="Url:IsBaseOf($pageUrl, Url:Initiator())">
				<xsl:attribute name="class">active</xsl:attribute>
			</xsl:if>-->
			
			<a href="{$pageUrl}" title="{$pageTitle}">
				<xsl:value-of select="$pageTitle"/>
			</a>
		</li>
	</xsl:template>
</xsl:stylesheet>
