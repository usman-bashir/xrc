<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<xsl:param name="genre"/>

	<xsl:template match="bookstore">
		<h1>
			Books by genre <xsl:value-of select="$genre"/>
		</h1>

		<table class="table table-striped">
			<thead>
				<TR>
					<th>ISBN</th>
					<th>Title</th>
					<th>Price</th>
				</TR>
			</thead>
			<tbody>
				<xsl:apply-templates select="book[@genre=$genre]"/>
			</tbody>
		</table>
	</xsl:template>
	<xsl:template match="book">
		<TR>
			<TD>
				<xsl:value-of select="@ISBN"/>
			</TD>
			<TD>
				<xsl:value-of select="title"/>
			</TD>
			<TD>
				<xsl:value-of select="price"/>
			</TD>
		</TR>
	</xsl:template>
</xsl:stylesheet>