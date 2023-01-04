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
import { onTicketDetailsByTicketId } from '../../../services/sales-service';
import { commaFormat, getTableTotal } from '../../../services/_utils';
import { getFormattedDateTime } from '../../../services/_utils/formatTime';
import InvoiceExport from '../../shared/InvoiceExport';
import Loading from '../../shared/Loading';

const InvoiceModal = ({ isOpen, onClose, ticketId }) => {
    const onSuccess = (data) => {};
    const onError = (err) => console.log(err);
    const { data, isLoading } = useQuery(
        ['ticketDetails', ticketId],
        () => onTicketDetailsByTicketId(ticketId),
        { onSuccess, onError }
    );
    const { Calculations, Orders, Payments, Table1, TicketEntities } = data?.data || {};

    const style = {
        table: {
            height: '815px',
            width: '575px',
            margin: '20px 10px ',
            position: 'relative',
            color: 'black'
        },
        fullLine: {
            width: '100%',
            height: '5px',
            background: '#29587e'
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
            marginTop: '60px'
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
        signature: {
            padding: '0px 20px 20px 0px'
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
                                            <td colSpan="6">Restuarant name</td>
                                        </tr>
                                        <tr>
                                            <td colSpan="6">Address</td>
                                        </tr>
                                    </table>

                                    <table style={style.mainHeadingTable}>
                                        <tr>
                                            <td colSpan="6">Sales Invoice</td>
                                        </tr>
                                        <tr style={style.fullLine}>
                                            <td colSpan="6"></td>
                                        </tr>
                                    </table>

                                    {Table1?.map((item, i) => (
                                        <table style={style.infoTable} key={i}>
                                            <tr>
                                                <td colSpan="3">
                                                    Client Name : {item['Client Name'] || ''}
                                                </td>
                                                <td colSpan="3">
                                                    Invoice No : {item?.TicketNo || ''}
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colSpan="3">
                                                    Department : {item?.Department || ''}
                                                </td>
                                                <td colSpan="3">
                                                    Invoice Date :{' '}
                                                    {getFormattedDateTime(item?.TicketDate) || ''}
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colSpan="3">Company : {item?.Company || ''}</td>
                                                <td colSpan="3">PO No : {item?.poNo || ''}</td>
                                            </tr>
                                            <tr>
                                                <td colSpan="3">Address : {item?.Address || ''}</td>
                                                <td colSpan="3">PO Date : {item?.poDate || ''}</td>
                                            </tr>
                                            <tr>
                                                <td colSpan="3">
                                                    Cell/Phone : {item?.Phone || ''}
                                                </td>
                                                <td colSpan="3">
                                                    Delivery Date :{' '}
                                                    {getFormattedDateTime(item?.DeliveryDate) || ''}
                                                </td>
                                            </tr>

                                            <tr>
                                                <td colSpan="3">Event : {item?.EventName || ''}</td>
                                                <td colSpan="3">
                                                    Outlet Name : {item?.outletName || ''}
                                                </td>
                                            </tr>
                                        </table>
                                    ))}

                                    <table style={style.itemTable}>
                                        <tr>
                                            <td style={style.th}>No.</td>
                                            <td style={style.th} colSpan="2">
                                                Item’s Description
                                            </td>
                                            <td style={style.th}>Quantity</td>
                                            <td style={style.th}>Rate(Tk.)</td>
                                            <td style={style.th}>Amount(Tk.)</td>
                                        </tr>

                                        {Orders?.map((item, i) => (
                                            <tr style={style.tr} key={i}>
                                                <td style={style.td}>{i + 1}</td>
                                                <td style={style.td} colSpan="2">
                                                    {item?.MenuItem || '-'}
                                                </td>
                                                <td style={style.td}>
                                                    {commaFormat(item?.Quantity || 0)}
                                                </td>
                                                <td style={style.td}>
                                                    {commaFormat(item?.UnitPrice || 0)}
                                                </td>
                                                <td style={style.td}>
                                                    {commaFormat(item?.Price || 0)}
                                                </td>
                                            </tr>
                                        ))}

                                        <tr>
                                            <td style={style.td}></td>
                                            <td style={style.td} colSpan="2"></td>
                                            <td style={style.td}></td>
                                            <td style={style.td}>Total</td>
                                            <td style={style.td}>
                                                {commaFormat(getTableTotal(Orders, 'Price'))}
                                            </td>
                                        </tr>
                                    </table>

                                    <table style={style.summaryTable}>
                                        {Calculations?.map((item, i) => (
                                            <tr key={i}>
                                                <td colSpan="4"></td>
                                                <td style={style.td}>{item?.Name || '-'}</td>
                                                <td style={style.td}>
                                                    {commaFormat(item?.CalculationAmount || 0)}
                                                </td>
                                            </tr>
                                        ))}

                                        <tr>
                                            <td colSpan={4}></td>
                                            <td style={style.td}>Total Vat :</td>
                                            {Table1?.map((item, i) => (
                                                <td style={style.td} key={i}>
                                                    {commaFormat(item?.TotalTax || 0)}
                                                </td>
                                            ))}
                                        </tr>

                                        <tr>
                                            <td colSpan={4}></td>
                                            <td style={style.td}>Grand Total:</td>
                                            {Table1?.map((item, i) => (
                                                <td style={style.td} key={i}>
                                                    {commaFormat(item?.TicketTotal || 0)}
                                                </td>
                                            ))}
                                        </tr>
                                    </table>

                                    <table style={style.footerTable}>
                                        <tr>
                                            <td colSpan="6" style={style.signature}>
                                                <strong>------------------------------</strong>
                                                <br />
                                                <strong>Authorized Signature</strong>
                                            </td>
                                        </tr>
                                        <tr style={style.fullLine}>
                                            <td colSpan="6"></td>
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

export default InvoiceModal;
