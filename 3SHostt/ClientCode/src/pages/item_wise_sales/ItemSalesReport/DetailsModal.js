import {
    Button,
    LightMode,
    Modal,
    ModalBody,
    ModalCloseButton,
    ModalContent,
    ModalFooter,
    ModalOverlay
} from '@chakra-ui/react';
import React from 'react';
import { useQuery } from 'react-query';
import { onItemSalesReportById } from '../../../services/item-wise-sales-services';
import { commaFormat, getTableTotal } from '../../../services/_utils';
import { getFormattedDateTime } from '../../../services/_utils/formatTime';
import InvoiceExport from '../../shared/InvoiceExport';
import Loading from '../../shared/Loading';

const DetailsModal = ({ isOpen, onClose, item }) => {
    const { ItemId, PortionName } = item || {};

    const { data, isLoading } = useQuery(['itemDetails', ItemId, PortionName], () =>
        onItemSalesReportById(ItemId, PortionName)
    );
    const ProductionCostDrill = data?.data?.ProductionCostDrill;

    const style = {
        table: {
            height: '815px',
            width: '575px',
            margin: '20px 10px ',
            position: 'relative',
            color: 'black'
        },
        topTable: {
            textAlign: 'right',
            width: '100%',
            fontSize: '10px',
            fontWeight: 'bold'
        },
        mainHeadingTable: {
            fontSize: '13px',
            fontWeight: 'bold',
            width: '100%',
            textAlign: 'center',
            marginTop: '30px'
        },
        infoTable: {
            width: '100%',
            fontSize: '10px',
            margin: '10px 0px'
        },
        itemTable: {
            width: '100%',
            margin: '20px 0px'
        },
        summaryTable: {
            width: '100%'
        },
        footerTable: {
            width: '100%',
            position: 'absolute',
            bottom: '10px',
            left: '0',
            fontSize: '10px',
            textAlign: 'right'
        },
        poweredBy: {
            textAlign: 'right'
        },
        logoImg: {
            width: '40px'
        },
        time: {
            textAlign: 'left'
        },
        th: {
            border: '1px solid gray',
            background: 'lightgray',
            fontWeight: 'bold',
            padding: '0px 5px',
            textAlign: 'center',
            fontSize: '10px'
        },
        td: {
            border: '1px solid gray',
            padding: '0px 5px',
            fontSize: '10px'
        }
    };

    return (
        <LightMode>
            <Modal isOpen={isOpen} onClose={onClose} size="full">
                <ModalOverlay />
                <ModalContent color="black">
                    <ModalCloseButton className="hidePrintArea" />

                    <ModalBody>
                        <InvoiceExport title="Ticket Reports">
                            {isLoading ? (
                                <Loading line={12} width="100%" />
                            ) : (
                                <table style={style.table} id="invoice-table-to-xls">
                                    <table style={style.topTable}>
                                        <tr>
                                            <td colSpan="5">Restuarant name</td>
                                        </tr>
                                        <tr>
                                            <td colSpan="5">Address</td>
                                        </tr>
                                    </table>

                                    <table style={style.mainHeadingTable}>
                                        <tr>
                                            <td colSpan="10">Production Cost Drill Through</td>
                                        </tr>
                                    </table>

                                    <table style={style.itemTable}>
                                        <tr>
                                            <td style={style.th}>Portion</td>
                                            <td style={style.th}>Fixed Cost</td>
                                            <td style={style.th}>Menu Item Name</td>
                                            <td style={style.th}>Recipe Name</td>
                                            <td style={style.th} colSpan="2">
                                                Inventory Item Name
                                            </td>
                                            <td style={style.th}>Quantity</td>
                                            <td style={style.th}>Base Unit</td>
                                            <td style={style.th}>Price</td>
                                            <td style={style.th}>Total</td>
                                        </tr>

                                        {ProductionCostDrill?.map((item, i) => (
                                            <tr style={style.tr} key={i}>
                                                <td style={style.td}>{item.PortionName}</td>
                                                <td style={style.td}>{item.FixedCost}</td>
                                                <td style={style.td}>{item.MenuItemName}</td>
                                                <td style={style.td}>{item.RecipeName}</td>
                                                <td style={style.td} colSpan="2">
                                                    {item.InventoryItemName}
                                                </td>
                                                <td style={style.td}>
                                                    {commaFormat(item.Quantity)}
                                                </td>
                                                <td style={style.td}>{item.BaseUnit}</td>
                                                <td style={style.td}>{commaFormat(item.Price)}</td>
                                                <td style={style.td}>{commaFormat(item.Total)}</td>
                                            </tr>
                                        ))}

                                        <tr style={style.tr}>
                                            <td style={style.td}></td>
                                            <td style={style.td}></td>
                                            <td style={style.td}></td>
                                            <td style={style.td}></td>
                                            <td style={style.td} colSpan="2"></td>
                                            <td style={style.td}></td>
                                            <td style={style.td}></td>
                                            <td style={style.td}>Total</td>
                                            <td style={style.td}>
                                                {commaFormat(
                                                    getTableTotal(ProductionCostDrill, 'Total')
                                                )}
                                            </td>
                                        </tr>
                                    </table>

                                    <table style={style.footerTable}>
                                        <tr>
                                            <td colSpan="5" style={style.time}>
                                                {getFormattedDateTime(new Date())}
                                            </td>

                                            <td colSpan="5" style={style.poweredBy}>
                                                <strong>Powered By</strong>
                                                <br />
                                                <img
                                                    style={style.logoImg}
                                                    src="../../../../../Images/3SLogo1.png"
                                                    alt=""
                                                />
                                            </td>
                                        </tr>
                                    </table>
                                </table>
                            )}
                        </InvoiceExport>
                    </ModalBody>

                    <ModalFooter className="hidePrintArea">
                        <Button variant="ghost" onClick={onClose}>
                            Close
                        </Button>
                    </ModalFooter>
                </ModalContent>
            </Modal>
        </LightMode>
    );
};

export default DetailsModal;
