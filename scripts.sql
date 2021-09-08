/* --------------------------------------------------- */
/* Ex 1 */
ALTER TABLE produto ADD ativo tinyint(1) DEFAULT 1 NOT NULL AFTER precoVenda;

/* 	Ex 2 */
SELECT p.cod AS 'CODIGO', 
	   p.descricao AS 'DESCRICAO', 
	   pg.nome AS 'GRUPO DO PRODUTO', 
       p.precoCusto AS 'CUSTO DO PRODUTO', 
       p.precoVenda AS 'PRECO DE VENDA' 
  FROM produto p 
  INNER JOIN produto_grupo pg
  ON p.codGrupo = pg.cod 
WHERE p.ativo = 1;

/* 	Ex 3 */
SELECT DATE_FORMAT(dataHora, '%d/%c/%Y') AS 'DATA', SUM(total) AS 'VALOR' 
	FROM venda 
    GROUP BY DATE_FORMAT(dataHora, '%d/%c/%Y');


SELECT p.descricao AS 'NOME PRODUTO', 
	   SUM(vp.quantidade) AS 'QUANTIDADE TOTAL', 
       (vp.quantidade * p.precoCusto) AS 'CUSTO TOTAL', 
       (vp.quantidade * p.precoVenda) AS 'VENDA TOTAL' 
   FROM produto p 
   INNER JOIN venda_produto vp 
   ON p.cod = vp.codProduto 
   INNER JOIN venda v 
   ON v.cod = vp.codVenda 
   WHERE v.dataHora = '2020-07-01 16:21:20'
   GROUP BY p.cod;
/* --------------------------------------------------- */

/* ------------ Utilidades para o sistema ------------ */
CREATE TABLE `log` (
  `cod` bigint(20) NOT NULL AUTO_INCREMENT,
  `message` varchar(150) NOT NULL,
  `userName` varchar(50) NOT NULL,
  `dateTime` datetime NOT NULL,
  PRIMARY KEY (`cod`)
);

CREATE TABLE `userLogin` (
	`cod` bigint(20) NOT NULL AUTO_INCREMENT,
    `credential` varchar(100) NOT NULL,
    `password` varchar(350) NOT NULL,
    PRIMARY KEY (`cod`)
);
/* --------------------------------------------------- */