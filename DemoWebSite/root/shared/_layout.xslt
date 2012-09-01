<xsl:stylesheet version="1.0"
                xmlns="http://www.w3.org/1999/xhtml"
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:Url="xrc:UrlModule"
                xmlns:Slot="xrc:SlotModule"
				xmlns:SourceCode="xrc:SourceCodeModule"
                exclude-result-prefixes="Url Slot">
	<xsl:output method="html" encoding="utf-8" />
	<xsl:param name="title" />
	<xsl:param name="activeMenu" />

	<xsl:template match="/">
		<xsl:text disable-output-escaping='yes'>&lt;!DOCTYPE html></xsl:text>
		<html>
			<head>
				<title>
					<xsl:value-of select="$title" />
				</title>
				<meta name="viewport" content="width=device-width, initial-scale=1.0" />

				<!-- jquery -->
				<script type="text/javascript" src="{Url:Content('~/scripts/jquery-1.7.2.min.js')}">//</script>

				<!-- bootstrap -->
				<link rel="Stylesheet" type="text/css" href="{Url:Content('~/content/bootstrap.min.css')}" />
				<link rel="Stylesheet" type="text/css" href="{Url:Content('~/content/bootstrap-responsive.min.css')}" />
				<script type="text/javascript" src="{Url:Content('~/scripts/bootstrap.min.js')}">
					<xsl:text xml:space="preserve"> </xsl:text>
				</script>

				<!-- knockoutjs -->
				<script type="text/javascript" src="{Url:Content('~/scripts/knockout-2.1.0.js')}">
					<xsl:text xml:space="preserve"> </xsl:text>
				</script>

				<link rel="stylesheet" type="text/css" href="{Url:Content('~/content/demo.css')}" />
			</head>
			<body>

				<xsl:variable name="headerParameters">
					<activeMenu>
						<xsl:value-of select="$activeMenu"/>
					</activeMenu>
				</xsl:variable>
				<xsl:value-of disable-output-escaping="yes" select="Slot:Include('~/shared/_header', $headerParameters )" />

				<div class="container-fluid">
					<xsl:value-of disable-output-escaping="yes" select="Slot:IncludeChild()" />

					<xsl:value-of disable-output-escaping="yes" select="Slot:Include('~/shared/_footer')" />
				</div>

				<div class="alert alert-info" id="showSourceCode">
					<a href="{SourceCode:GetGitLink()}" target="_blank">
						<img src="{Url:Content('~/content/images/github.ico')}" />
						show github source
					</a>
				</div>

			</body>
		</html>
	</xsl:template>
</xsl:stylesheet>