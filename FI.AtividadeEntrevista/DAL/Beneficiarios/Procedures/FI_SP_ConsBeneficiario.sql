﻿

CREATE OR ALTER PROCEDURE FI_SP_ConsBeneficiario
	@IDCLIENTE BIGINT=NULL
AS
BEGIN
	IF(ISNULL(@IDCLIENTE,0) = 0)
		SELECT ID, NOME, CPF, IDCLIENTE FROM BENEFICIARIOS WITH(NOLOCK)
	ELSE
		SELECT ID, NOME, CPF, IDCLIENTE FROM BENEFICIARIOS WITH(NOLOCK) WHERE IDCLIENTE = @IDCLIENTE
END
GO