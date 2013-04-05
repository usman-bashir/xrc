<xsl:stylesheet version="1.0"
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output omit-xml-declaration="yes"/>
	<xsl:template match="bookstore">
    <h3>Xslt</h3>
    <p>This is a xslt view</p>    
		<table>
			<thead>
				<TR>
					<th>ISBN</th>
					<th>Title</th>
					<th>Price</th>
					<th>Genre</th>
				</TR>
			</thead>
			<tbody>
				<xsl:apply-templates select="book"/>
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
			<TD>
				<xsl:value-of select="@genre"/>
			</TD>
		</TR>
	</xsl:template>
</xsl:stylesheet>