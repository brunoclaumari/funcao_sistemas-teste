
let idCliente;
// “ID”, “CPF”, “NOME”, “IDCLIENTE
let listaBeneficiarios = [];

$(document).ready(function () {
    $('#Cpf').mask('000.000.000-00');
    $('#CpfBeneficiario').mask('000.000.000-00');

    $('#CEP').mask('00000-000');

});

//$('#Cpf').on('keyup', function () {
//    console.log($('#Cpf').val());
//});

//.on('show.bs.modal'
$('#btn_beneficiarios').on('click', function () {

    $('#modal_beneficiarios').modal('show');
});

//Ações ao abrir o modal.
$('#modal_beneficiarios').on('shown.bs.modal', function () {
    console.log('Modal foi aberto');
    idCliente = $('#Id').val();
    console.log(`Id cliente atual: ${idCliente}`);
});

//Ações ao fechar o modal.
$('#modal_beneficiarios').on('hide.bs.modal', function () {



});


//IncluirBeneficiario
$('#IncluirBeneficiario').on('click', function () {
    
    let tabelaBody = document.getElementById('contentBeneficiarios');

    var linhas = tabelaBody.getElementsByTagName('tr');

    tabelaBody.innerHTML += AdicionaBeneficiarioTabela(linhas.length + 1);

    document.getElementById('CpfBeneficiario').value = '';
    document.getElementById('NomeBeneficiario').value = '';

});

function PreencheModalBeneficiarios(listaObjBeneficiarios) {

    let tabelaBody = document.getElementById('contentBeneficiarios');
    tabelaBody.innerHTML = "";

    listaObjBeneficiarios.map((benef, index) => {
        let id = benef.Id;
        let cpf = benef.Cpf;
        let nome = benef.Nome;
        tabelaBody.innerHTML += AdicionaLinhaBeneficiarioTabela(index+1, cpf, nome, id);
        //let idCliente = benef.IdCliente;
    });
}

//contentBeneficiarios
//Beneficiarios
function ObtemListaBeneficiariosDadosTela() {
    listaBeneficiarios = [];
    let beneficiarios = document.getElementById('contentBeneficiarios');
    let linhaTabela = beneficiarios.getElementsByTagName('tr');
    for (var i = 0; i < linhaTabela.length; i++) {
        var count = i + 1
        let elementosTr = linhaTabela[i].getElementsByTagName('td');
        let beneficiario = {
            id: elementosTr[0].textContent,
            cpf: elementosTr[1].textContent,
            nome: elementosTr[2].textContent,
        };
        listaBeneficiarios.push(beneficiario);
    }
    console.log(`Lista: ${listaBeneficiarios}`);

    return listaBeneficiarios;
}

function AlterarBeneficiario(count_position) {
    let CpfBeneficiario = $('#CpfBeneficiario').val();
    let NomeBeneficiario = $('#NomeBeneficiario').val();        

    let nodeCpf = document.getElementById(`cpf_${count_position}`);
    nodeCpf.textContent = CpfBeneficiario;

    document.getElementById(`nome_${count_position}`).textContent = NomeBeneficiario;  
    
    document.getElementById('CpfBeneficiario').value = '';
    document.getElementById('NomeBeneficiario').value = '';
}

function RetirarBeneficiario(count_position) {
    var id = `linha_${count_position}`;

    // Removendo um nó a partir do pai
    var node = document.getElementById(id);
    if (node.parentNode) {
        node.parentNode.removeChild(node);
    }
}

function PreencheSelecionado(count_position) {

    $('#CpfBeneficiario').val(document.getElementById(`cpf_${count_position}`).textContent);
    $('#NomeBeneficiario').val(document.getElementById(`nome_${count_position}`).textContent); 

}


function AdicionaBeneficiarioTabela(count) {
    //let count = $("#contentBeneficiarios tbody tr").length;

    let CpfBeneficiario = $('#CpfBeneficiario').val();
    let NomeBeneficiario = $('#NomeBeneficiario').val();

    return AdicionaLinhaBeneficiarioTabela(count, CpfBeneficiario, NomeBeneficiario, 0);

    //return `<tr id="linha_${count}" >
    //            <td id="id_${count}" class="hidden">0</td>
    //            <td id="cpf_${count}" >${CpfBeneficiario}</td>
    //            <td id="nome_${count}" >${NomeBeneficiario}</td>
    //            <td>
    //                <button id="alterarBeneficiario_${count}" onclick="AlterarBeneficiario(${count})" type="button" class="btn btn-primary">Alterar</button>
    //                <button id="excluirBeneficiario_${count}" onclick="RetirarBeneficiario(${count})" type="button" class="btn btn-primary">Excluir</button>
    //            </td>
    //        </tr>`;
}

function AdicionaLinhaBeneficiarioTabela(index, cpfBeneficiario, nomeBeneficiario, idBeneficiario) {
    let count = index;           

    return `<tr id="linha_${count}" onclick="PreencheSelecionado(${count})">
                <td id="id_${count}" class="hidden">${idBeneficiario}</td>
                <td id="cpf_${count}" >${cpfBeneficiario}</td>
                <td id="nome_${count}" >${nomeBeneficiario}</td>
                <td>
                    <button id="alterarBeneficiario_${count}" onclick="AlterarBeneficiario(${count})" type="button" class="btn btn-primary">Alterar</button>
                    <button id="excluirBeneficiario_${count}" onclick="RetirarBeneficiario(${count})" type="button" class="btn btn-primary">Excluir</button>
                </td>
            </tr>`;
}